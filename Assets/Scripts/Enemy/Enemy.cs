using DamageNumbersPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//需要挂载的组件,挂载Enemy后自动添加
[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    //protected 子类可以访问
    [HideInInspector]public Animator anim;

    [HideInInspector] public Animator magicAttackAnim;

    private AnimatorStateInfo animatorStateInfo;

    [HideInInspector]public PhysicsCheck physicsCheck;

    [HideInInspector] public EnemyMagicAttack enemyMagicAttack;

    private Collider2D coll2d;

    private Animator hitAnimator;

    public DamageNumber damageNumber;

    public ParticleSystem particleSystem;

    public GameObject deathRipple;

    protected Material _material;


    [Header("基本参数")]
    public float normalSpeed;

    public float chaseSpeed;

    [HideInInspector] public float currentSpeed;

    public Vector3 faceDir;

    public float hurtForce;
    //攻击者
    public Transform attacker;
    //出生点
    public Vector3 spwanPoint;

    [Header("检测")]
    public Vector2 centerOffset;

    public Vector2 checkSize;
    //发现玩家距离
    public float checkDistance;


    public LayerMask attackLayer;

    [Header("计时器")]
    public float waitTime;

    public float waitTimeCounter;

    public bool wait;

    public float lostTime;

    public float lostTimeCounter;

    [Header("状态")]
    public bool isHurt;

    public bool isDead;

    private BaseState currentState;

    protected BaseState patrolState;

    protected BaseState chaseState;

    protected BaseState skillState;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll2d = GetComponent<CapsuleCollider2D>();
        enemyMagicAttack = GetComponent<EnemyMagicAttack>();
        //获得子物体受击动画的动画器
        if(transform.childCount > 0)
            hitAnimator = transform.GetChild(0).GetComponent<Animator>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
        spwanPoint = transform.position;
        _material = GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {

        
        //当面朝墙且碰到墙时再转身
        currentState.LogicUpdate();
        TimeCounter();




    }

    //刚体放在fixedUpdate执行
    private void FixedUpdate()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if (!isHurt && !isDead && !wait)
        {
            Move();
            currentState.PhysicsUpdate();
        }
        
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    //virtual表示可以被重写
    public virtual void Move()
    {
        //如果当前没有播放蜗牛的premove动画，则移动,GetCurrentAnimatorStateInfo(0)表示第0个layer，即animation中的baseLayer
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("snailPreMove") && !anim.GetCurrentAnimatorStateInfo(0).IsName("snailRecover"))
        {
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
        }
            
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
                //因为faceDir=-transform.localScale.x，所以这里转身不用负值（因为速度方向和transform.localScale.x是反的，所以faceDir初始化时=-transform.localScale.x）
                //模型转身
                transform.localScale = new Vector3(faceDir.x, 1, 1);
                //方向变量转向
                faceDir = new Vector3(-transform.localScale.x, 0, 0);
                //重新检测点
                physicsCheck.Check();
                waitTimeCounter = waitTime;
                //Debug.Log("Enemy.!currentEnemy.physicsCheck.isGround" + !this.physicsCheck.isGround );
            }
        }
        //如果没有发现玩家，则将追击等待时间减少
        if (!FoundPlayer() && lostTimeCounter > 0 )
        {
            lostTimeCounter -= Time.deltaTime;
        }

    }

    //发现玩家
    public virtual bool FoundPlayer() 
    {
        //向前方发射一个盒子检测射线，从中心点transform.position + (Vector3)centerOffset，向方向为faceDir发射一个大小为checkSize角度为0的射线，射线长度为checkDistance，检测对象为attackLayer
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset,checkSize,0,faceDir,checkDistance,attackLayer);

    }

    //生成魔法攻击
    public void MagicAttack() 
    {
        StartCoroutine(MagicAttackGenerate());
    }

    private IEnumerator MagicAttackGenerate()
    {
        //Debug.Log("this.attacker.transform.position:" + this.attacker.transform.position);
        //协程，在执行完抬手待0.75s再执行下一步
        yield return new WaitForSeconds(0.75f);
        //远程攻击
        enemyMagicAttack.AfterMagicAttack(attacker);
    }

    public void SwitchState(NPCState state)
    {
        //根据传进来的枚举值state判断currentState切换到哪种状态
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null
        };
        //当前状态退出
        currentState.OnExit();
        //切换到新状态
        currentState = newState;
        currentState.OnEnter(this);
    }

    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }

    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans) 
    {
        attacker = attackTrans;
        //受击转身
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

        #region 受击特效
        hitAnimator.transform.position = new Vector2(this.transform.position.x, attackTrans.position.y + 1.2f);
        //如果攻击者是player,才播放受击特效
        if(attackTrans.CompareTag("Player"))
            hitAnimator.SetTrigger("Hit");
        #endregion

        //弹幕伤害
        damageNumber.Spawn(transform.position + new Vector3(0,2,0), attackTrans.GetComponent<Attack>().damage);
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        //受伤时先将野猪停下，再被击退
        rb.velocity = new Vector2(0, rb.velocity.y);
        //执行协程
        StartCoroutine(OnHurt(dir));
        
    }

    private IEnumerator OnHurt(Vector2 dir) 
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //协程，在执行完击退后等待0.5s再执行下一步
        yield return new WaitForSeconds(0.3f);
        isHurt = false;
    }

    public virtual void OnDie(Transform attackTrans) 
    {

        
        //设置当前敌人图层为ignore Raycast，在project setting中的physics2d设置人物不与ignore Raycast产生碰撞
        gameObject.layer = 2;


        if (!anim.GetBool("isDead"))
        {
            particleSystem.transform.position = new Vector2(transform.position.x, transform.position.y + coll2d.offset.y);
            particleSystem.transform.localScale = new Vector3 ((this.transform.position.x - attacker.transform.position.x) > 0 ? 1 : -1,1,1);
            //未修改好
            //particleSystem.GetComponent<ParticleSystem>().shape.rotation.Set(0f, 55f * (this.transform.position.x - attacker.transform.position.x) > 0 ? 1 : -1, 0f);
            #region 粒子特效
            var s = DOTween.Sequence();
            
            s.AppendCallback(() =>
            {
                particleSystem.Play();

            });
            s.Append(_material.DOFloat(600, "_Strength", 0.05f));
            s.AppendCallback(() =>
            {
                //涟漪特效
                deathRipple.GetComponent<RippleEffect>().Ripple(transform.position, transform.localScale, coll2d.offset);

            });

            #endregion
        }
        anim.SetBool("isDead", true);
        isDead = true;
        //particleSystem.Play();

    }

    //死亡之后销毁此敌人
    public void DestoryAfterAnimation() 
    {
        Destroy(this.gameObject);
    }
    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        //在场景中绘制检测范围（碰撞范围可视化）
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * faceDir.x,0), 0.2f);
    }
}
