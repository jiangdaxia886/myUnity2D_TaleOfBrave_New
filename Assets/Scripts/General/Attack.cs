using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    public float attackRange;

    public float attackRate;

    //������һ��������ײ�����
    private void OnTriggerStay2D(Collider2D collision)
    {
        //����ʾ���������characterִ����δ��룬û�в�ִ��
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
