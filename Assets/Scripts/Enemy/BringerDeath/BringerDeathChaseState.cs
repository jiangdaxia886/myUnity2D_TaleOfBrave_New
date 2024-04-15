using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BringerDeathChaseState : BaseState
{
    private bool isAttack;
    private float attackRateCounter = 0;
    private Attack attack;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        //currentEnemy.anim.SetBool("chase", true);
        attack = enemy.GetComponent<Attack>();
        //更改速度
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
    }
    public override void LogicUpdate()
    {
        //当追击计时<0时，结束追击，切换状态
        //子弹击中后被销毁，所以currentEnemy.attacker == null时也要切换回巡逻状态
        if (currentEnemy.lostTimeCounter <= 0 || currentEnemy.attacker == null)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
            return;
        }
        //当面朝墙且碰到墙时再转身
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //Debug.Log("111111"+ !currentEnemy.physicsCheck.isGround);
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
            // 移动方向
            //currentEnemy.faceDir = new Vector3(-(currentEnemy.attacker.position.x - currentEnemy.transform.position.x), 0, 0).normalized;
        }
        //玩家与enemy距离
        float distince = (currentEnemy.attacker.position - currentEnemy.transform.position).magnitude;


        //判断距离
        //如果距离比远程攻击距离远，则追击
        if (distince > attack.remoteDistanceMax)
        {
            //超出攻击范围则取消攻击状态
            isAttack = false;
            currentEnemy.anim.SetBool("walk", true);
        }
        //远程攻击
        else if (distince >= attack.remoteDistanceMin && distince <= attack.remoteDistanceMax)
        {
            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                currentEnemy.anim.SetTrigger("magicAttack");
                //远程攻击
                //执行协程
                currentEnemy.MagicAttack();
                //重置攻击时间
                attackRateCounter = attack.attackRate;
            }
        }
        //到达攻击范围先停下
        else if (distince < attack.remoteDistanceMin)
        {
            
            //近战
            isAttack = true;
            //如果受伤状态，则可以被击退，如果不是受伤状态，则到达攻击范围停止
            if (!currentEnemy.isHurt)
            currentEnemy.rb.velocity = Vector2.zero;
            currentEnemy.anim.SetBool("walk", false);
            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                //重置攻击时间
                attackRateCounter = attack.attackRate;
                currentEnemy.anim.SetTrigger("nearAttack");
            }
        }
    }



    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
            currentEnemy.rb.velocity = new Vector2(currentEnemy.currentSpeed * currentEnemy.faceDir.x * Time.deltaTime, currentEnemy.rb.velocity.y);
        }
    }

    public override void OnExit()
    {
        

    }


}
