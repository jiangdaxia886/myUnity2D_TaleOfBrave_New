using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : Enemy
{
    [Header("移动范围")]
    public float patrolRadius;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }
    public override bool FoundPlayer()
    {
        //判断以transform.position为中心点，半径为checkDistance范围内是否有attackLayer
        var obj =  Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj) 
        {
            //将attacker设置为玩家
            attacker = obj.transform;
        }
        return obj;
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);
        //返回出生点周围的随机点
        return spwanPoint + new Vector3(targetX, targetY);
    }

    public override void Move()
    {
        
    }
}
