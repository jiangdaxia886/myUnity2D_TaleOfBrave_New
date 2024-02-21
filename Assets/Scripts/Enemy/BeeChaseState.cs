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
        //玩家的中心点是在脚底，因为切割的时候选的是bottom，所以目标点的y轴需要添加1.5f
        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1f, 0);
        //到达攻击范围先停下
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.attackRange && Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {
            isAttack = true;
            //如果受伤状态，则可以被击退，如果不是受伤状态，则到达攻击范围停止
            if(!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;
            //攻击
            //计时器
            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                //重置攻击时间
                attackRateCounter = attack.attackRate;
                currentEnemy.anim.SetTrigger("attack");
            }
        }
        else 
        {
            //超出攻击范围则取消攻击状态
            isAttack = false;
        }
        // 移动方向
        moveDir = (target - currentEnemy.transform.position).normalized;
        //转身,面朝目标方向
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
