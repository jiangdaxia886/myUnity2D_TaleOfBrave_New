using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new SnailPartrolState();
        skillState = new SnailSkillState();
    }
}
