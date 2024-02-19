using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;

    public float CurrentHealth;

    [Header("�����޵�")]
    //�����޵�ʱ��
    public float invulnerableDuration;
    //���������޵�ʱ��
    public float activeInvulnerableDuration;
    //�޵м�ʱ��
    [HideInInspector] public float invulnerableCounter;
    //�޵�״̬
    public bool invulnerable;

    //�����¼�
    public UnityEvent<Transform> onTakeDamage;
    //�����¼�
    public UnityEvent onDie;
    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        //������޵�״̬���޵�ʱ�����
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            //�޵�ʱ��������˳��޵�״̬
            if (invulnerableCounter <= 0) 
            {
                invulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attacker) 
    {
        if (invulnerable)
        {
            return;
        }
        //Debug.Log(attacker.damage);
        if (CurrentHealth - attacker.damage > 0)
        {
            CurrentHealth -= attacker.damage;
            TriggerInvulnerable();
            //ִ������(�����ǰ��ɫ�����onTakeDamage�¼�����ִ��)
            onTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            CurrentHealth = 0;
            onDie?.Invoke();
        }

    }
    //���������޵�״̬
    public void ActiveInvulnerable() 
    {
        if (activeInvulnerableDuration > invulnerableCounter)
        {
            invulnerable = true;
            invulnerableCounter = activeInvulnerableDuration;
        }
    }

    private void TriggerInvulnerable() 
    {
        //��������޵�״̬������ô˷��������޵�
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
