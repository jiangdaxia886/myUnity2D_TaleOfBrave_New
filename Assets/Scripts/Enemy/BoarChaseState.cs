using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        //Debug.Log("chase");
        //更改速度
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);
    }
    public override void LogicUpdate()
    {
        //当追击计时<0时，结束追击，切换状态
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        //当面朝墙且碰到墙时再转身
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //Debug.Log("111111"+ !currentEnemy.physicsCheck.isGround);
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x ,1,1);
        }

    }


    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
        
    }
}
