using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{

    private Vector3 target;
    private Vector3 moveDir;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        //获得当前移动的目标点
        target = enemy.GetNewPoint();
        //Debug.Log("target:" + target.x + target.y);
    }
    public override void LogicUpdate()
    {
        //发现player切换到Chase
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        //如果到达目标点则等待并寻找下一个目标位置
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) < 0.1f && Mathf.Abs(target.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.wait = true;
            target = currentEnemy.GetNewPoint();
        }
        //移动方向
        moveDir = (target - currentEnemy.transform.position).normalized;
        //转身,面朝目标方向
        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
    }




    public override void PhysicsUpdate()
    {
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }    
    }


    public override void OnExit()
    {
        
    }
}
