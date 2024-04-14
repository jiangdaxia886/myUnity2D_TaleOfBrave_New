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
        //�ж���transform.positionΪ���ĵ㣬�뾶ΪcheckDistance��Χ���Ƿ���attackLayer
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
        {
            //��attacker����Ϊ���
            attacker = obj.transform;
        }
        return obj;
    }

}
