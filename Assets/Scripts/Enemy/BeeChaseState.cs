using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;

    private Attack attack;

    private bool isAttack;

    private float attackRateCounter = 0;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = enemy.GetComponent<Attack>();
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("chase", true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        //��ҵ����ĵ����ڽŵף���Ϊ�и��ʱ��ѡ����bottom������Ŀ����y����Ҫ���1.5f
        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1f, 0);
        //���﹥����Χ��ͣ��
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.attackRange && Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {
            isAttack = true;
            //�������״̬������Ա����ˣ������������״̬���򵽴﹥����Χֹͣ
            if(!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;
            //����
            //��ʱ��
            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                //���ù���ʱ��
                attackRateCounter = attack.attackRate;
                currentEnemy.anim.SetTrigger("attack");
            }
        }
        else 
        {
            //����������Χ��ȡ������״̬
            isAttack = false;
        }
        // �ƶ�����
        moveDir = (target - currentEnemy.transform.position).normalized;
        //ת��,�泯Ŀ�귽��
        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
    }



    public override void PhysicsUpdate()
    {
        if ( !currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", false);
    }


}
