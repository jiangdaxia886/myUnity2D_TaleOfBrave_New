using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
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
        currentPower = maxPower;
        OnHealthChange.Invoke(this);
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnregisterSaveData();
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

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        //��������������е�ǰ��������꣬����ģ�û�������
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSaveData[GetDataID().ID + "health"] = this.CurrentHealth;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower;
        }
        else
        {
            //����λ��
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            //��������
            data.floatSaveData.Add(GetDataID().ID + "health", this.CurrentHealth);
            //����power
            data.floatSaveData.Add(GetDataID().ID + "power", this.currentPower);
        }
    }

    public void LoadSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];
            this.CurrentHealth = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];
            //����Ѫ��
            OnHealthChange.Invoke(this);
        }
    }
}
