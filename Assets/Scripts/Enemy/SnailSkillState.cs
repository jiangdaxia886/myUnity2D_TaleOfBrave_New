
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
        //����skill����֮�����ö�ʧʱ�䣬�ڶ�ʧʱ��>0ʱ�������л���Patrol״̬
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        //����skill״̬ʱ�������޵�״̬
        currentEnemy.GetComponent<Character>().invulnerable = true;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        //�����޵�ʱ��
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
