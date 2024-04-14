using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerDeathEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BringerDeathPatrolState();
        chaseState = new BringerDeathChaseState();
    }

    public override bool FoundPlayer()
    {
        //判断以transform.position为中心点，半径为checkDistance范围内是否有attackLayer
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
        {
            //将attacker设置为玩家
            attacker = obj.transform;
        }
        return obj;
    }

}
