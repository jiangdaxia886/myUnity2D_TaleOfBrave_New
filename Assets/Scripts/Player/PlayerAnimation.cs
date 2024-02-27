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
        //��ȡ�������
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
        //����x�ٶȲ���
        anim.SetFloat("velocityX", Mathf.Abs( rb.velocity.x));
        //����Y�ٶȲ���
        anim.SetFloat("velocityY", rb.velocity.y);
        //����isGround����
        //Debug.Log("rb.velocity.y" + rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isDead", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
        anim.SetBool("isSlide", playerController.isSlide);
        //anim.SetInteger("combo", playerController.combo);
        anim.SetBool("onWall", physicsCheck.onWall);

    }

    //��character���е�onTakeDamage�¼�������˴˷���
    public void PlayHurt() 
    {
        anim.SetTrigger("Hurt");
    }

    //��playerController��awake()�����ڹ����������µ�ʱ�����
    public void PlayAttack() 
    {
        anim.SetTrigger("attack");
    }

    public void PlaySlide()
    {
        anim.SetTrigger("slide");
    }
}
