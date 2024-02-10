using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //��unity�Զ����ɵ�PlayerInputController��������
    public PlayerInputController inputController;
    //������
    private Rigidbody2D rb;
    //�Զ������ײ���
    private PhysicsCheck physicsCheck;
    //��ȡPlayerAnimation��
    private PlayerAnimation playerAnimation;
    //��ȡCapsuleCollider2D��
    private CapsuleCollider2D capsuleCollider;
    
    private Character character;


    //��ǰλ��
    public Vector2 inputDirection;

    [Header("��������")]
    public float speed;

    public float slideSpeed;

    public float jumpForce;

    public float hurtForce;

    public float slideSize;

    public float slideOffsety;

    public float capsuleCollider2Dy;

    public float capsuleCollider2DOffsety;



    //public int combo;

    [Header("�������")]
    public PhysicsMaterial2D normal;

    public PhysicsMaterial2D wall;

    [Header("״̬")]

    public bool isHurt;

    public bool isDead;

    public bool isAttack;

    public bool isSlide;
    //��������
    public float slideDirection;
    //���ﳯ��
    public float playerDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //����Զ������ײ�����
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        character = GetComponent<Character>();
        //ʵ������������
        inputController = new PlayerInputController();
        //started��������ȥ��һ����Ծ,��jump������Ϊ�¼��������µ���һ����ִ��
        inputController.GamePlay.Jump.started += Jump;
        //����
        inputController.GamePlay.Attack.started += PlayerAttack;
        //����
        inputController.GamePlay.Slide.started += Slide;

    }



    private void OnEnable()
    {
        inputController.Enable();
    }

    private void OnDisable()
    {
        inputController.Disable();
    }

    //Update()����ÿһ֡����ִ��
    private void Update()
    {
        //������뷽��ֵ
        inputDirection = inputController.GamePlay.Move.ReadValue<Vector2>();
        //��õ�ǰ�泯����
        if(inputDirection.x != 0)
            playerDirection = inputDirection.x;
        CheckState();
    }

    //�������fixedUpdateִ��
    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
        //�����ƶ�
        Move();
    }

    //����
    //�����ڴ�������Χ�ڻ�һֱ���
/*    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }*/

    public void Move() {
        //�����ƶ�,x���ٶ��Ǽ���ģ�y���ٶ���ԭ�е�9.81�������ٶ�
        //������ڻ���״̬���������ƶ�����ת������ڣ�����٣��Ҳ��ܷ�ת
        Debug.Log("inputDirection.x"+inputDirection.x);
        if (!isSlide)
        {
            //���﷭ת
            int faceDir = (int)transform.localScale.x;

            if (inputDirection.x > 0)
                faceDir = 1;
            if (inputDirection.x < 0)
                faceDir = -1;
            transform.localScale = new Vector3(faceDir, 1, 1);
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }
        //����
        else
        {
            //���﷭ת
            int faceDir = (int)transform.localScale.x;

            if (slideDirection > 0)
                faceDir = 1;
            if (slideDirection < 0)
                faceDir = -1;
            transform.localScale = new Vector3(faceDir, 1, 1);
            rb.velocity = new Vector2(slideDirection * slideSpeed * Time.deltaTime, rb.velocity.y);
        }
            



    }

    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump");
        if(physicsCheck.isGround)
        //impulse ���һ��˲ʱ����
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    //����
    private void Slide(InputAction.CallbackContext context) 
    {
        //Debug.Log("isSlide" + !this.isSlide);
        if (!isSlide && physicsCheck.isGround)
        {
            isSlide = true;
            //������������泯����
            slideDirection = playerDirection;
            //���������޵�
            character.ActiveInvulnerable();
            //������ײ�壬����ʱ�䰫
            capsuleCollider.size = new Vector2(capsuleCollider.size.x, slideSize);
            capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, slideOffsety);
            playerAnimation.PlaySlide();

        }

    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
        //Debug.Log("isAttack");
        /*combo++;
        if(combo >= 4)
            combo = 0;*/
    }


    #region UnityEvent
    //��character���е�onTakeDamage�¼�������˴˷���
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        //�ý�ɫλ�ü�ȥ����λ�ã��õ������ٹ�һ������ԽԶ��ԽС
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    //��character���е�onDie�¼�������˴˷���
    public void PlayerDead() 
    {
        isDead = true;
        //����֮��رտ�������gameplay���ֵĲ���
        inputController.GamePlay.Disable();
    }
    #endregion

    //���ڵ�����ʱ��Ħ�������ʣ����ڵ������ù⻬����
    public void CheckState()
    {
        capsuleCollider.sharedMaterial = physicsCheck.isGround?normal:wall;
    }
}