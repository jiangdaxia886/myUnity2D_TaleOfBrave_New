using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    //protected ������Է���
    protected Animator anim;

    PhysicsCheck physicsCheck;

    [Header("��������")]
    public float normalSpeed;

    public float chaseSpeed;

    public float currentSpeed;

    public Vector3 faceDir;

    public float hurtForce;
    //������
    public Transform attacker;

    [Header("��ʱ��")]
    public float waitTime;

    public float waitTimeCounter;

    public bool wait;

    [Header("״̬")]
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

        //���泯ǽ������ǽʱ��ת��
        if(physicsCheck.touchLeftWall && faceDir.x < 0|| physicsCheck.touchRightWall && faceDir.x > 0)
        {
            wait = true;
            anim.SetBool("walk", false);
            
        }
        TimeCounter();
    }

    //�������fixedUpdateִ��
    private void FixedUpdate()
    {
        if (!isHurt && !isDead)
        {
            Move();
        }
        
    }

    //virtual��ʾ���Ա���д
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime,rb.velocity.y);
    }

    public void TimeCounter()
    {
        if(wait) 
        { 
            waitTimeCounter -= Time.deltaTime;
            //�ȴ�ʱ�������ת��
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
        //ת��
        //��������Ұ���Ҳ࣬��Ұ�����ң�Ұ��Ĭ��-1�����ң�1����
        if (attackTrans.position.x - transform.position.x > 0) 
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //���˱�����
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        //ִ��Э��
        StartCoroutine(OnHurt(dir));
        
    }

    private IEnumerator OnHurt(Vector2 dir) 
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //Э�̣���ִ������˺�ȴ�0.5s��ִ����һ��
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    public void OnDie() 
    {
        //���õ�ǰ����ͼ��Ϊignore Raycast����project setting�е�physics2d�������ﲻ��ignore Raycast������ײ
        gameObject.layer = 2;
        anim.SetBool("isDead", true);
        isDead = true;

    }

    //����֮�����ٴ˵���
    public void DestoryAfterAnimation() 
    {
        Destroy(this.gameObject);
    }
}
