
using UnityEngine;

public class SnailSkillState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetTrigger("skill");
        currentEnemy.anim.SetBool("hide", true);
        //进入skill动画之后重置丢失时间，在丢失时间>0时，不会切换至Patrol状态
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        //进入skill状态时，进入无敌状态
        currentEnemy.GetComponent<Character>().invulnerable = true;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        //重置无敌时间
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.GetComponent<Character>().invulnerableDuration;
    }




    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.GetComponent<Character>().invulnerable = false;
    }
}
