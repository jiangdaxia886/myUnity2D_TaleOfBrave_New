using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    public float attackRange;

    public float attackRate;

    //是否持续伤害
    public bool isDurationAttack;

    //传进来一个其他碰撞体对象
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDurationAttack)
        {
            //？表示如果对面有character执行这段代码，没有不执行
            collision.GetComponent<Character>()?.TakeDamage(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果不是持续伤害
        if (!isDurationAttack)
        {

            //？表示如果对面有character执行这段代码，没有不执行
            collision.GetComponent<Character>()?.TakeDamage(this);
        }
    }
}
