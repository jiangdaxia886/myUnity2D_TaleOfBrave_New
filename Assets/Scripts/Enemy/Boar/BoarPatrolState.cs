
using UnityEngine;

public class BoarPatrolState : BaseState
{


    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("walk", true);
    }

    public override void LogicUpdate()
    {
        //����player�л���chase
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        //���泯ǽ������ǽʱ��ǰ����������ת��
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            Debug.Log("currentEnemy.physicsCheck.touchLeftWall" + currentEnemy.physicsCheck.touchLeftWall+ ";currentEnemy.faceDir.x:"+ currentEnemy.faceDir.x);
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
        currentEnemy.anim.SetBool("walk", false);
        //Debug.Log("patrol exit");
    }
}
