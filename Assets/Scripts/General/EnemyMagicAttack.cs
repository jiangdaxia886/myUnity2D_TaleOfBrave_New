using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicAttack : MonoBehaviour
{
    [Header("Ԥ����")]
    public GameObject magicAttack;

    public Vector2 magicPosition;




    public void AfterMagicAttack(Transform Attacker)
    {
        //���������Զ�̹���
        GameObject attackEffect = ObjectPool.Instance.GetObject(magicAttack);
        attackEffect.transform.position = (Vector2)Attacker.transform.position + magicPosition;
    }
}
