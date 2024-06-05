using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class PlayerController : MonoBehaviour
{
    [Header("�¼�����")]
    //������������¼�
    public SceneLoadEventSo sceneLoadEvent;

    public VoidEventSo afterSceneLoadedEvent;
    //������restart��ť������Ϸ)����isdead��false
    public VoidEventSo loadDataEvent;
    //������backToMenuEvent��ť������Ϸ)����isdead��false
    public VoidEventSo backToMenuEvent;


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
    //��Ӱ
    public GameObject dashObj;
    //Ripple
    public GameObject rippleEffect;

    public GameObject dashRippleEffect;

    private SpriteRenderer spriteRenderer;

    public GameObject effects;

    public EffectManager effectManager;



    //��ǰλ��
    public Vector2 inputDirection;

    [Header("��������")]
    public float speed;

    private float runSpeed;

    //=>��ʾwalkSpeedʱrunSpeed��һ��
    private float walkSpeed => speed / 2.5f;

    public float slideSpeed;
    //������������
    public int slidePowerCost;

    public float jumpForce;

    public float jumpVelocity;

    private float jumpSpeed;

    //��Ծ���ʱ��
    public float jumpDura;
    //��Ծ��ʱ��
    private float jumpTimer;

    public float onWallJumpForce;

    public float hurtForce;

    public float slideSize;

    public float slideOffsety;
    //����ʱ��
    public float JumpGraceTime;


    //��ȡ��ʼ��ײ���С
    [HideInInspector] public float capsuleCollider2Dy;

    [HideInInspector] public float capsuleCollider2DOffsety;

    //public int test;


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

    public bool wallJump;

    private float beforeJumpTimer;

    private JumpState jumpState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //����Զ������ײ�����
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        character = GetComponent<Character>();
        effectManager = effects.GetComponent<EffectManager>();
        //spriteRenderer = this.GetComponent<SpriteRenderer>();
        capsuleCollider2Dy = capsuleCollider.size.y;
        capsuleCollider2DOffsety = capsuleCollider.offset.y;
        //ʵ������������
        inputController = new PlayerInputController();
        //started��������ȥ��һ����Ծ,��jump������Ϊ�¼��������µ���һ����ִ��
        //inputController.GamePlay.Jump.started += Jump;
        inputController.GamePlay.Jump.performed += Jump;
        inputController.GamePlay.Jump.canceled += ctx =>
        {
                jumpSpeed = 0;
        };
        inputController.Enable();
        //����
        inputController.GamePlay.Attack.started += PlayerAttack;
        //����
        inputController.GamePlay.Slide.started += Slide;
        //�����а�����סʱ(lambda���ʽ,����һ���ص�����)
        runSpeed = speed;
        inputController.GamePlay.WalkButton.performed += ctx => 
        {
            if (physicsCheck.isGround) 
            {
                speed = walkSpeed;
            }
        };
        //�����а�������ʱ
        inputController.GamePlay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };
        inputController.Enable();

    }



    private void OnEnable()
    {
        
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        //�������˵�����Ҳ���︴��
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }


    private void OnDisable()
    {
        inputController.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        //�������˵�����Ҳ���︴��
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }



    //�������fixedUpdateִ��
    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
            //�����ƶ�
            Move();
        CheckState();
        //����ʱ�䣨JumpTimer > 0 ʱ������Ծ��
        if (physicsCheck.isGround || physicsCheck.onWall)
        {
            beforeJumpTimer = JumpGraceTime;
            //��¼����״̬
            jumpState = physicsCheck.isGround ? JumpState.GroundJump : JumpState.WallJump;
        }
        else if(beforeJumpTimer > 0)
        {
            beforeJumpTimer -= Time.deltaTime;
        }
        if (jumpTimer > 0 )
            jumpTimer -= Time.deltaTime;


    }

    //Update()����ÿһ֡����ִ��
    private void Update()
    {
        //������뷽��ֵ
        inputDirection = inputController.GamePlay.Move.ReadValue<Vector2>();
        //��õ�ǰ�泯����
        if(inputDirection.x != 0)
            playerDirection = inputDirection.x;
        //CheckState();
    }

    private void LateUpdate()
    {
/*        if (wallJump)
        {
            test++;
            Debug.Log("playerController.LateUpdate:" + test);
        }*/
    }



    //����
    //�����ڴ�������Χ�ڻ�һֱ���
    /*    private void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log(collision.name);
        }*/

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        //��������ʱ�����������
        inputController.GamePlay.Disable();
        //Debug.Log("��������ʱ�����������");
    }

    //�����¼�������ʱisDead��false
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        //�������غ�ָ��������
        inputController.GamePlay.Enable();
        //Debug.Log("�������غ�ָ��������");
    }

    public void Move() {
        //�����ƶ�,x���ٶ��Ǽ���ģ�y���ٶ���ԭ�е�9.81�������ٶ�
        //������ڻ���״̬���������ƶ�����ת������ڣ�����٣��Ҳ��ܷ�ת
        //Debug.Log("inputDirection.x"+inputDirection.x);
        if (!isSlide)
        {
            //������Ч��Ӱ��δ�޸�bug��
            //dashObj.SetActive(false);
/*            dashObj.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
            dashObj.transform.localScale = this.transform.localScale;*/
            //���﷭ת
            int faceDir = (int)transform.localScale.x;

            if (inputDirection.x > 0)
                faceDir = 1;
            if (inputDirection.x < 0)
                faceDir = -1;
            transform.localScale = new Vector3(faceDir, 1, 1);
            //��ǽ��״̬ʱ�����ܿ���
            if ( !wallJump)
            {
                //�������Ծ����y���ٶȵ�����Ծ�ٶ�
                if (jumpSpeed != 0 && jumpTimer > 0)
                    rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, jumpSpeed * Time.deltaTime);
                else
                    rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
            }
                
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
            //��Ӱ
            ShadowPool.instance.GetFormPool();
            //������Ч
            if(physicsCheck.isGround)
                effectManager.WallSlide(this.transform.position + new Vector3(inputDirection.x, 0, 0) * 0.5f, new Vector2(0, 1));
            /*            dashObj.SetActive(true);
                        dashObj.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
                        dashObj.transform.localScale = this.transform.localScale;*/

            transform.localScale = new Vector3(faceDir, 1, 1);
            //�������Ծ����y���ٶȵ�����Ծ�ٶ�
            if (jumpSpeed != 0 && jumpTimer > 0)
                rb.velocity = new Vector2(slideDirection * slideSpeed * Time.deltaTime, jumpSpeed * Time.deltaTime);
            else
                rb.velocity = new Vector2(slideDirection * slideSpeed * Time.deltaTime, rb.velocity.y);

        }
            



    }

    private void Jump(InputAction.CallbackContext context)
    {

        //����ʱ��>0���ǵ�������
        if (beforeJumpTimer > 0 && jumpState == JumpState.GroundJump)
        {
            jumpTimer = jumpDura;
            jumpSpeed = jumpVelocity;
            //impulse ���һ��˲ʱ����
            //rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip();
            Debug.Log("jump!!!");
            //������Ч
            effectManager.JumpDust(this.transform.position, Vector2.up);
        }
        //����ʱ��>0���ǵ�ǽ��
        else if (beforeJumpTimer > 0 && jumpState == JumpState.WallJump)
        {
            //jumpTimer = jumpDura;
            //��Ϊ��˲ʱ������Ϊ�˱�֤��Ծ�߶�һ��������Ծ֮ǰ�ٶ���Ϊ0
            rb.velocity = Vector2.zero;
            //��ǽ����x�᷽���ٶ�ΪinputDirection.x�ķ�����
            rb.AddForce(new Vector3(-0.45f * inputDirection.x, 2f,0) * onWallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
            //������Ч
            effectManager.JumpDust(this.transform.position + (Vector3)inputDirection * 0.5f + new Vector3(0,0.3f,0), -inputDirection);
            /*             Debug.Log("Time.time"+Time.time);
                     if (wallJump)
                       {
                           test++;
                           Debug.Log("playerController.input.jump:" + test);
                       }*/
        }

    }

    //����
    private void Slide(InputAction.CallbackContext context) 
    {
        //Debug.Log("isSlide" + !this.isSlide);
        if (!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost)
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
            character.OnSlide(slidePowerCost);
            //������Ч
            dashRippleEffect.GetComponent<DashRippleEffect>().DashRipple(transform.position, transform.localScale);

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

    public void FreezeFrame()
    {
        //����ʱ��֡
        GetComponent<FrameFrozen>()?.frazee(0.05f);
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
/*        if (wallJump)
        {
            test++;
            Debug.Log("playerController.FixUpdate:" + test + "  rb.velocity.y:" + rb.velocity.y + "    physicsCheck.onWall" + physicsCheck.onWall);
            Debug.Log("playerController.FixUpdate.touchLeftWall:" + physicsCheck.touchLeftWall + "   playerController.inputDirection.x:" + inputDirection.x + "   phychveloy: " + physicsCheck.phychveloy);

        }*/
        capsuleCollider.sharedMaterial = physicsCheck.isGround?normal:wall;
        //�»�����,ͬʱ����walljump״̬ʱ�ż�������Ϊonwall�ļ������ײ��ͻ����������walljumpʱҲ�ᴦ��onwall���ᵼ�µ�ǽ��������
        if (physicsCheck.onWall && !wallJump)
        {
            //������Ч
            effectManager.WallSlide(this.transform.position + new Vector3(inputDirection.x,0,0) * 0.5f + new Vector3(0,1,0) ,-inputDirection);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.5f);
        }
        
        //y���ٶ�С��7ʱ��ǽ��Ϊfalse,���ҿ��Բٿ��ƶ�
        if (wallJump && rb.velocity.y < 7f)
        {
            wallJump = false;
        }
    }
}