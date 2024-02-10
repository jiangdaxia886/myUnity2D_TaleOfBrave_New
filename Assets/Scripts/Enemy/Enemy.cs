using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    //protected 子类可以访问
    protected Animator anim;

    PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;

    public float chaseSpeed;

    public float currentSpeed;

    public Vector3 faceDir;

    public float hurtForce;
    //攻击者
    public Transform attacker;

    [Header("计时器")]
    public float waitTime;

    public float waitTimeCounter;

    public bool wait;

    [Header("状态")]
    public bool isHurt;

    public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x,0,0);

        //当面朝墙且碰到墙时再转身
        if(physicsCheck.touchLeftWall && faceDir.x < 0|| physicsCheck.touchRightWall && faceDir.x > 0)
        {
            wait = true;
            anim.SetBool("walk", false);
            
        }
        TimeCounter();
    }

    //刚体放在fixedUpdate执行
    private void FixedUpdate()
    {
        if (!isHurt && !isDead)
        {
            Move();
        }
        
    }

    //virtual表示可以被重写
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime,rb.velocity.y);
    }

    public void TimeCounter()
    {
        if(wait) 
        { 
            waitTimeCounter -= Time.deltaTime;
            //等待时间结束后转身
            if (waitTimeCounter <= 0) 
            { 
                wait = false;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
                waitTimeCounter = waitTime;
            }
        }
    }

    public void OnTakeDamage(Transform attackTrans) 
    {
        attacker = attackTrans;
        //转身
        //如果玩家在野猪右侧，则野猪向右（野猪默认-1是向右，1向左）
        if (attackTrans.position.x - transform.position.x > 0) 
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //受伤被击退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        //执行协程
        StartCoroutine(OnHurt(dir));
        
    }

    private IEnumerator OnHurt(Vector2 dir) 
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //协程，在执行完击退后等待0.5s再执行下一步
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    public void OnDie() 
    {
        //设置当前敌人图层为ignore Raycast，在project setting中的physics2d设置人物不与ignore Raycast产生碰撞
        gameObject.layer = 2;
        anim.SetBool("isDead", true);
        isDead = true;

    }

    //死亡之后销毁此敌人
    public void DestoryAfterAnimation() 
    {
        Destroy(this.gameObject);
    }
}
