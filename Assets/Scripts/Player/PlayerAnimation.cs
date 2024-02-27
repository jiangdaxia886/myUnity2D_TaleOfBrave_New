using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;

    private void Awake()
    {
        //获取动画组件
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation() 
    {
        //设置x速度参数
        anim.SetFloat("velocityX", Mathf.Abs( rb.velocity.x));
        //设置Y速度参数
        anim.SetFloat("velocityY", rb.velocity.y);
        //设置isGround参数
        //Debug.Log("rb.velocity.y" + rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isDead", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
        anim.SetBool("isSlide", playerController.isSlide);
        //anim.SetInteger("combo", playerController.combo);
        anim.SetBool("onWall", physicsCheck.onWall);

    }

    //在character类中的onTakeDamage事件中添加了此方法
    public void PlayHurt() 
    {
        anim.SetTrigger("Hurt");
    }

    //在playerController中awake()方法在攻击按键按下的时候调用
    public void PlayAttack() 
    {
        anim.SetTrigger("attack");
    }

    public void PlaySlide()
    {
        anim.SetTrigger("slide");
    }
}
