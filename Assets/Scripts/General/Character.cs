using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VoidEventSo newGameEvent;

    [Header("基本属性")]
    public float maxHealth;

    public float CurrentHealth;

    public float maxPower;

    public float currentPower;

    public float powerRecoverSpeed;

    [Header("受伤无敌")]
    //设置无敌时间
    public float invulnerableDuration;
    //设置主动无敌时间
    public float activeInvulnerableDuration;
    //无敌计时器
    [HideInInspector] public float invulnerableCounter;
    //无敌状态
    public bool invulnerable;


    //血量变化事件
    public UnityEvent<Character> OnHealthChange;
    //受伤事件
    public UnityEvent<Transform> onTakeDamage;
    //死亡事件
    public UnityEvent onDie;
    //受伤音效广播
    public PlayAudioEventSO playAudioEvent;
    //受伤音效
    public AudioClip audioClip;

    //加载新场景重置血量
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

        if (currentPower < maxPower) 
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }

    //掉入水中，死亡
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
            //执行受伤(如果当前角色添加了onTakeDamage事件，则执行)
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
        //如果保存数据中有当前物体的坐标，则更改，没有则添加
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSaveData[GetDataID().ID + "health"] = this.CurrentHealth;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower;
        }
        else
        {
            //保存位置
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            //保存生命
            data.floatSaveData.Add(GetDataID().ID + "health", this.CurrentHealth);
            //保存power
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
            //更新血条
            OnHealthChange.Invoke(this);
        }
    }
}
