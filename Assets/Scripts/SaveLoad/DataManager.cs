using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;

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

    //��ȡ�洢·��
    private string jsonFolder;

    //����ģʽ
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //Data��û�м̳�monobehaviour��û���������ڹ���������ʹ��ʱ��Ҫnew
        saveData = new Data();
        //��ȡ������ļ���
        jsonFolder = Application.persistentDataPath + "/SAVE DATA/";
        //��ȡ������ʼ�
        ReadSaveData();
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

        //��չ��дΪʲô���ɣ�����д��.sav
        var resultPath = jsonFolder + "data.sav";
        //��saveDataת��ΪString���͵�����
        var jsonData = JsonConvert.SerializeObject(saveData);
        //�ж��Ƿ��иñ����ļ�������У���ֱ��д�룬���û�У��򴴽�Ŀ¼
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        //д�ļ�
        File.WriteAllText(resultPath, jsonData);
    }

    public void Load()
    {
        //������foreach��������Ϊsaveables���޸��ˣ���ԭ�������л�����ʱ���ﱻ���ú͹ر��ˣ���ʱcharacter�л���ע��saveables�Ĳ���
        foreach (var saveable in saveables)
        {
            saveable.LoadSaveData(saveData);
        }
    }

    private void ReadSaveData() 
    {

        //��չ��дΪʲô���ɣ�����д��.sav
        var resultPath = jsonFolder + "data.sav";
        //�ж��Ƿ��иñ����ļ�������У���ֱ�Ӷ������û�У��򴴽�Ŀ¼
        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);
            //����ȡ���ļ������л�ΪData
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }
}
