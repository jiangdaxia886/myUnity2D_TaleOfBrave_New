using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicAttack : MonoBehaviour
{
    [Header("预制体")]
    public GameObject magicAttack;

    public Vector2 magicPosition;




    public void AfterMagicAttack(Transform Attacker)
    {
        //对象池生成远程攻击
        GameObject attackEffect = ObjectPool.Instance.GetObject(magicAttack);
        attackEffect.transform.position = (Vector2)Attacker.transform.position + magicPosition;
    }
}
