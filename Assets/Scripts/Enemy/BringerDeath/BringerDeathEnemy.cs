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
        //向前方发射一个盒子检测射线，从中心点transform.position + (Vector3)centerOffset，向方向为faceDir发射一个大小为checkSize角度为0的射线，射线长度为checkDistance，检测对象为attackLayer
        var obj = Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
        if (obj)
        {
            //将attacker设置为玩家
            attacker = obj.transform;
        }
        return obj;
    }
    public override void Move()
    {

    }
}
