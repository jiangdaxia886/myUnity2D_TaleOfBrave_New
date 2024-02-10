using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    public float attackRange;

    public float attackRate;

    //传进来一个其他碰撞体对象
    private void OnTriggerStay2D(Collider2D collision)
    {
        //？表示如果对面有character执行这段代码，没有不执行
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
