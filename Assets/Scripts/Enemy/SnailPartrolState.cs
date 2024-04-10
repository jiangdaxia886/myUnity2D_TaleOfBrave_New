using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPartrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("walk", true);
    }

    public override void LogicUpdate()
    {
        //����player�������л���skill
        if (currentEnemy.FoundPlayer() || currentEnemy.isHurt)
        {
            currentEnemy.SwitchState(NPCState.Skill);
        }
        //���泯ǽ������ǽʱ��ǰ����������ת��
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //Debug.Log("111111"+ !currentEnemy.physicsCheck.isGround);
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }


    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
    }

}
