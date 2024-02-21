using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : Enemy
{
    [Header("�ƶ���Χ")]
    public float patrolRadius;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }
    public override bool FoundPlayer()
    {
        //�ж���transform.positionΪ���ĵ㣬�뾶ΪcheckDistance��Χ���Ƿ���attackLayer
        var obj =  Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj) 
        {
            //��attacker����Ϊ���
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
        //���س�������Χ�������
        return spwanPoint + new Vector3(targetX, targetY);
    }

    public override void Move()
    {
        
    }
}
