using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;

    public float CurrentHealth;

    [Header("受伤无敌")]
    //设置无敌时间
    public float invulnerableDuration;
    //设置主动无敌时间
    public float activeInvulnerableDuration;
    //无敌计时器
    [HideInInspector] public float invulnerableCounter;
    //无敌状态
    public bool invulnerable;

    //受伤事件
    public UnityEvent<Transform> onTakeDamage;
    //死亡事件
    public UnityEvent onDie;
    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        //如果在无敌状态，无敌时间减少
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            //无敌时间结束，退出无敌状态
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
            //执行受伤(如果当前角色添加了onTakeDamage事件，则执行)
            onTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            CurrentHealth = 0;
            onDie?.Invoke();
        }

    }
    //主动进入无敌状态
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
        //如果不在无敌状态，则调用此方法触发无敌
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
