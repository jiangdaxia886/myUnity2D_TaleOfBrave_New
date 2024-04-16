using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerDeathPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("walk", true);
        //Debug.Log("BringerDeathPatrolState::::::");
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
            //Debug.Log("Snail111111"+ !currentEnemy.physicsCheck.isGround);
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
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !currentEnemy.wait)
        {
            currentEnemy.rb.velocity = new Vector2(currentEnemy.currentSpeed * currentEnemy.faceDir.x * Time.deltaTime, currentEnemy.rb.velocity.y);
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
        //Debug.Log("patrol exit");
    }
}
