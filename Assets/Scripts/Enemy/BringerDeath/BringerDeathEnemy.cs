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
        //��ǰ������һ�����Ӽ�����ߣ������ĵ�transform.position + (Vector3)centerOffset������ΪfaceDir����һ����СΪcheckSize�Ƕ�Ϊ0�����ߣ����߳���ΪcheckDistance��������ΪattackLayer
        var obj = Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
        if (obj)
        {
            //��attacker����Ϊ���
            attacker = obj.transform;
        }
        return obj;
    }
    public override void Move()
    {

    }
}
