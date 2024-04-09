using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ҫ���ص����,����Enemy���Զ����
[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    //protected ������Է���
    [HideInInspector]public Animator anim;

    [HideInInspector]public PhysicsCheck physicsCheck;

    private Collider2D coll2d;


    [Header("��������")]
    public float normalSpeed;

    public float chaseSpeed;

    [HideInInspector] public float currentSpeed;

    public Vector3 faceDir;

    public float hurtForce;
    //������
    public Transform attacker;
    //������
    public Vector3 spwanPoint;

    [Header("���")]
    public Vector2 centerOffset;

    public Vector2 checkSize;

    public float checkDistance;

    public LayerMask attackLayer;

    [Header("��ʱ��")]
    public float waitTime;

    public float waitTimeCounter;

    public bool wait;

    public float lostTime;

    public float lostTimeCounter;

    [Header("״̬")]
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
        coll2d = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
        spwanPoint = transform.position;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x,0,0);

        //���泯ǽ������ǽʱ��ת��
        currentState.LogicUpdate();
        TimeCounter();
        
    }

    //�������fixedUpdateִ��
    private void FixedUpdate()
    {
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

    //virtual��ʾ���Ա���д
    public virtual void Move()
    {
        //�����ǰû�в�����ţ��premove���������ƶ�GetCurrentAnimatorStateInfo(0)��ʾ��0��layer����animation�е�baseLayer
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
            //�ȴ�ʱ�������ת��
            if (waitTimeCounter <= 0) 
            { 
                wait = false;
                //��ΪfaceDir=-transform.localScale.x����������ת���ø�ֵ����Ϊ�ٶȷ����transform.localScale.x�Ƿ��ģ�����faceDir��ʼ��ʱ=-transform.localScale.x��
                transform.localScale = new Vector3(faceDir.x, 1, 1);
                waitTimeCounter = waitTime;
            }
        }
        //���û�з�����ң���׷���ȴ�ʱ�����
        if (!FoundPlayer() && lostTimeCounter > 0 )
        {
            lostTimeCounter -= Time.deltaTime;
        }

    }

    //�������
    public virtual bool FoundPlayer() 
    {
        //��ǰ������һ�����Ӽ�����ߣ������ĵ�transform.position + (Vector3)centerOffset������ΪfaceDir����һ����СΪcheckSize�Ƕ�Ϊ0�����ߣ����߳���ΪcheckDistance��������ΪattackLayer
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset,checkSize,0,faceDir,checkDistance,attackLayer);

    }

    public void SwitchState(NPCState state)
    {
        //���ݴ�������ö��ֵstate�ж�currentState�л�������״̬
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null
        };
        //��ǰ״̬�˳�
        currentState.OnExit();
        //�л�����״̬
        currentState = newState;
        currentState.OnEnter(this);
    }

    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }

    #region �¼�ִ�з���
    public void OnTakeDamage(Transform attackTrans) 
    {
        attacker = attackTrans;
        //�ܻ�ת��
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
        //����ʱ�Ƚ�Ұ��ͣ�£��ٱ�����
        rb.velocity = new Vector2(0, rb.velocity.y);
        //ִ��Э��
        StartCoroutine(OnHurt(dir));
        
    }

    private IEnumerator OnHurt(Vector2 dir) 
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        //Э�̣���ִ������˺�ȴ�0.5s��ִ����һ��
        yield return new WaitForSeconds(0.3f);
        isHurt = false;
    }

    public virtual void OnDie() 
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
    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        //�ڳ����л��Ƽ�ⷶΧ����ײ��Χ���ӻ���
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * faceDir.x,0), 0.2f);
    }
}
