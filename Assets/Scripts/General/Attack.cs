using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    public float attackRange;

    public float attackRate;

    //�Ƿ�����˺�
    public bool isDurationAttack;

    //������һ��������ײ�����
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDurationAttack)
        {
            //����ʾ���������characterִ����δ��룬û�в�ִ��
            collision.GetComponent<Character>()?.TakeDamage(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //������ǳ����˺�
        if (!isDurationAttack)
        {

            //����ʾ���������characterִ����δ��룬û�в�ִ��
            collision.GetComponent<Character>()?.TakeDamage(this);
        }
    }
}
