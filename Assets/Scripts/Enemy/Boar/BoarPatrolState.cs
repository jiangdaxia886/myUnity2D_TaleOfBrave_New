
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
        //发现player切换到chase
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        //当面朝墙且碰到墙时或前方是悬崖再转身
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
