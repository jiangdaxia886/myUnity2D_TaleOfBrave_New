using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//����������ִ��
[DefaultExecutionOrder(-100)]
//Ŀǰ���ؽ������ַ�ʽ��һ����Update()���ö�ȡ���̰���ִ��load()����һ���ڽ���button�����ִ���¼�loadDataEvent��ִ��
public class DataManager : MonoBehaviour
{
    //��̬������Ϸһ��ʼ�ͻ�洢���ڴ���
    public static DataManager instance;

    [Header("�¼�����")]
    //������Ϸ
    public VoidEventSo saveDataEvent;
    //������Ϸ������restart��ť������Ϸ��
    public VoidEventSo loadDataEvent;
    //��ʵ��ISaveable����ע�����
    public List<ISaveable> saveables = new List<ISaveable>();

    private Data saveData;

    //����ģʽ
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //Data��û�м̳�monobehaviour��û���������ڹ���������ʹ��ʱ��Ҫnew
        saveData = new Data();
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    //��ȡ������L�������ر��������
    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }



    //���û�д˶�������ӱ���
    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveables.Contains(saveable))
        {
            saveables.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        if (saveables.Contains(saveable))
        {
            saveables.Remove(saveable);
        }
    }


    public void Save()
    {

        //��������ʵ��saveable����
        foreach (var saveable in saveables)
        {
            saveable.GetSaveData(saveData);
        }

        /*foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + ":::" + item.Value);
        }*/
    }

    public void Load()
    {
        //������foreach��������Ϊsaveables���޸��ˣ���ԭ�������л�����ʱ���ﱻ���ú͹ر��ˣ���ʱcharacter�л���ע��saveables�Ĳ���
        foreach (var saveable in saveables)
        {
            saveable.LoadSaveData(saveData);
        }
        /*        for (int i = 0; i <= saveables.Count; i++)
                {
                    saveables[i].LoadSaveData(saveData);
                }*/
    }
}
