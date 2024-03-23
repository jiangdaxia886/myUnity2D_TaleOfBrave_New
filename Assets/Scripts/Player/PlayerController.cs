using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("事件监听")]
    public SceneLoadEventSo loadEvent;

    public VoidEventSo afterSceneLoadedEvent;

    //用unity自动生成的PlayerInputController控制器类
    public PlayerInputController inputController;
    //刚体类
    private Rigidbody2D rb;
    //自定义的碰撞检测
    private PhysicsCheck physicsCheck;
    //获取PlayerAnimation类
    private PlayerAnimation playerAnimation;
    //获取CapsuleCollider2D类
    private CapsuleCollider2D capsuleCollider;
    
    private Character character;


    //当前位置
    public Vector2 inputDirection;

    [Header("基本参数")]
    public float speed;

    private float runSpeed;

    //=>表示walkSpeed时runSpeed的一半
    private float walkSpeed => speed / 2.5f;

    public float slideSpeed;
    //滑铲力度消耗
    public int slidePowerCost;

    public float jumpForce;

    public float onWallJumpForce;

    public float hurtForce;

    public float slideSize;

    public float slideOffsety;

    //获取初始碰撞体大小
    [HideInInspector] public float capsuleCollider2Dy;

    [HideInInspector] public float capsuleCollider2DOffsety;

    private int test;


    //public int combo;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;

    public PhysicsMaterial2D wall;

    [Header("状态")]

    public bool isHurt;

    public bool isDead;

    public bool isAttack;

    public bool isSlide;
    //滑铲方向
    public float slideDirection;
    //人物朝向
    public float playerDirection;

    public bool wallJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //获得自定义的碰撞检测类
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        character = GetComponent<Character>();
        capsuleCollider2Dy = capsuleCollider.size.y;
        capsuleCollider2DOffsety = capsuleCollider.offset.y;
        //实例化控制器类
        inputController = new PlayerInputController();
        //started按键按下去那一刻跳跃,将jump方法作为事件按键按下的那一刻来执行
        inputController.GamePlay.Jump.started += Jump;
        //攻击
        inputController.GamePlay.Attack.started += PlayerAttack;
        //滑铲
        inputController.GamePlay.Slide.started += Slide;
        //当步行按键按住时(lambda表达式,传入一个回调函数)
        runSpeed = speed;
        inputController.GamePlay.WalkButton.performed += ctx => 
        {
            if (physicsCheck.isGround) 
            {
                speed = walkSpeed;
            }
        };
        //当步行按键不按时
        inputController.GamePlay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };

    }



    private void OnEnable()
    {
        inputController.Enable();
        loadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }


    private void OnDisable()
    {
        inputController.Disable();
        loadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    //刚体放在fixedUpdate执行
    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
            //人物移动
            Move();
        //CheckState();

    }

    //Update()方法每一帧都会执行
    private void Update()
    {
        //获得输入方向值
        inputDirection = inputController.GamePlay.Move.ReadValue<Vector2>();
        //获得当前面朝方向
        if(inputDirection.x != 0)
            playerDirection = inputDirection.x;
        CheckState();
    }

    private void LateUpdate()
    {
        if (wallJump)
        {
            test++;
            Debug.Log("LateUpdate:" + test);
        }
    }



    //测试
    //人物在触发器范围内会一直检测
    /*    private void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log(collision.name);
        }*/

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        //场景加载时禁用人物操作
        inputController.GamePlay.Disable();
        //Debug.Log("场景加载时禁用人物操作");
    }


    private void OnAfterSceneLoadedEvent()
    {
        //场景加载后恢复人物操作
        inputController.GamePlay.Enable();
        //Debug.Log("场景加载后恢复人物操作");
    }

    public void Move() {
        //人物移动,x轴速度是计算的，y轴速度是原有的9.81重力加速度
        //如果不在滑铲状态，则正常移动及翻转，如果在，则加速，且不能翻转
        //Debug.Log("inputDirection.x"+inputDirection.x);
        if (!isSlide)
        {
            //人物翻转
            int faceDir = (int)transform.localScale.x;

            if (inputDirection.x > 0)
                faceDir = 1;
            if (inputDirection.x < 0)
                faceDir = -1;
            transform.localScale = new Vector3(faceDir, 1, 1);
            //蹬墙跳状态时方向不受控制
            if ( !wallJump)
                rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }
        //滑铲
        else
        {
            //人物翻转
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
        if (physicsCheck.isGround)
        {
            //impulse 添加一个瞬时的力
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip();
            /* Debug.Log("physicsCheck.isGround:" + physicsCheck.isGround);
            Debug.Log("physicsCheck.onWall:" + physicsCheck.onWall);*/
        }
        else if (physicsCheck.onWall)
        {
            //因为是瞬时的力，为了保证跳跃高度一样，在跳跃之前速度置为0
            rb.velocity = Vector2.zero;
            //蹬墙跳，x轴方向速度为inputDirection.x的反方向
            rb.AddForce(new Vector3(-0.45f * inputDirection.x, 2f,0) * onWallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
            Debug.Log("Time.time"+Time.time);
            if (wallJump)
            {
                test++;
                Debug.Log("jump:" + test);
            }
        }

    }

    //滑铲
    private void Slide(InputAction.CallbackContext context) 
    {
        //Debug.Log("isSlide" + !this.isSlide);
        if (!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost)
        {
            isSlide = true;
            //滑铲方向等于面朝方向
            slideDirection = playerDirection;
            //滑铲人物无敌
            character.ActiveInvulnerable();
            //设置碰撞体，滑铲时变矮
            capsuleCollider.size = new Vector2(capsuleCollider.size.x, slideSize);
            capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, slideOffsety);
            playerAnimation.PlaySlide();
            character.OnSlide(slidePowerCost);

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
    //在character类中的onTakeDamage事件中添加了此方法
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        //让角色位置减去敌人位置，得到方向，再归一化，即越远力越小
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    //在character类中的onDie事件中添加了此方法
    public void PlayerDead() 
    {
        isDead = true;
        //死亡之后关闭控制器的gameplay部分的操作
        inputController.GamePlay.Disable();
    }
    #endregion

    //当在地面上时用摩擦力材质，不在地面上用光滑材质
    public void CheckState()
    {
        if (wallJump)
        {
            test++;
            Debug.Log("Update:" + test);
            Debug.Log("rb.velocity.y:" + rb.velocity.y+ "    physicsCheck.onWall"+ physicsCheck.onWall);
            Debug.Log("transform.localScale.x:"+ transform.position.x);

        }
        capsuleCollider.sharedMaterial = physicsCheck.isGround?normal:wall;
        //下滑减慢,同时不是walljump状态时才减慢，因为onwall的监测点比碰撞体突出，所以在walljump时也会处于onwall，会导致蹬墙跳跳不高
        if (physicsCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.5f);
        
        //y轴速度小于7时蹬墙跳为false
        if (wallJump && rb.velocity.y < 7f)
        {
            wallJump = false;
        }
    }
}