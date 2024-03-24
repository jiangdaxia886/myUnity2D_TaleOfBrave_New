using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider;

    [Header("检测参数")]
    public bool manual;

    public bool isPlayer;

    private PlayerController playerController;

    private Rigidbody2D rb;
    //碰撞点偏移量
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    //碰撞范围
    public float checkRaduis;
    //碰撞图层
    public LayerMask groundLayer;

    [Header("状态")]
    public Boolean isGround;

    public bool touchLeftWall;

    public bool touchRightWall;

    public bool onWall;

    public float phychveloy;


    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //如果不勾选手动设置
        if (!manual)
        {
            //右侧碰撞点的x偏移量等于碰撞体的范围/2+碰撞体的偏移量，y轴位置是y的边界/2
            rightOffset = new Vector2((capsuleCollider.bounds.size.x / 2 + capsuleCollider.offset.x) + 0.1f, (capsuleCollider.bounds.size.y / 2));
            //左侧的x轴偏移量等于右侧偏移量的负值
            leftOffset = new Vector2((-capsuleCollider.bounds.size.x / 2 - capsuleCollider.offset.x) - 0.1f, rightOffset.y);
        }
        if (isPlayer)
        {
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }
        Check();
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    private void Check()
    {
        //在墙壁上时添加地面检测点的偏移量，使碰撞体接触地面时才检测为true
        if (onWall)
            //检测地面,以transform.position + bottomOffset为中心点，半径为checkRaduis范围内是否存在groundLayer图层,此处加了new Vector2(bottomOffset.x * transform.localScale.x,bottomOffset.y)，表示当前敌人前方脚下位置作为检测点，当野猪前方是悬崖时则不往前走
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);
        
        else
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRaduis, groundLayer);

        //墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);

        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);

        //在墙壁上
        if (isPlayer)
        {
/*            phychveloy = rb.velocity.y;
            playerController.test++;
            Debug.Log("physicsCheck.playerController.test:" + playerController.test+ "   physicsCheck.phychveloy"+  phychveloy);*/
            //下落时且贴着墙并按对应方向键
            onWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && (rb.velocity.y < 0f);
        }

    }

    private void OnDrawGizmosSelected()
    {
        //在场景中绘制检测范围（碰撞范围可视化）
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }

}