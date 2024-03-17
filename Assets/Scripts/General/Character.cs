using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("�¼�����")]
    public VoidEventSo newGameEvent;

    [Header("��������")]
    public float maxHealth;

    public float CurrentHealth;

    public float maxPower;

    public float currentPower;

    public float powerRecoverSpeed;

    [Header("�����޵�")]
    //�����޵�ʱ��
    public float invulnerableDuration;
    //���������޵�ʱ��
    public float activeInvulnerableDuration;
    //�޵м�ʱ��
    [HideInInspector] public float invulnerableCounter;
    //�޵�״̬
    public bool invulnerable;


    //Ѫ���仯�¼�
    public UnityEvent<Character> OnHealthChange;
    //�����¼�
    public UnityEvent<Transform> onTakeDamage;
    //�����¼�
    public UnityEvent onDie;
    //������Ч�㲥
    public PlayAudioEventSO playAudioEvent;
    //������Ч
    public AudioClip audioClip;

    //�����³�������Ѫ��
    private void NewGame()
    {
        CurrentHealth = maxHealth;
        OnHealthChange.Invoke(this);
        currentPower = maxPower;
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
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

        if (currentPower < maxPower) 
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }

    //����ˮ�У�����
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            CurrentHealth = 0;
            OnHealthChange?.Invoke(this);
            onDie?.Invoke();
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
            playAudioEvent.RaiseEvent(audioClip);
        }
        else 
        {
            CurrentHealth = 0;
            onDie?.Invoke();
        }
        OnHealthChange.Invoke(this);

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

    public void OnSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }
}
