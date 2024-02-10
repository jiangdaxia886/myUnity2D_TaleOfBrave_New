using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarEnemy : Enemy
{
    public override void Move()
    {
        base.Move();
        anim.SetBool("walk",true);
    }
}
