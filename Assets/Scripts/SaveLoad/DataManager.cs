using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;

//加载数据先执行
[DefaultExecutionOrder(-100)]
//目前加载进度两种方式，一种在Update()调用读取键盘按键执行load()，另一中在界面button中添加执行事件loadDataEvent来执行
public class DataManager : MonoBehaviour
{
    //静态对象，游戏一开始就会存储在内存中
    public static DataManager instance;

    [Header("事件监听")]
    //保存游戏
    public VoidEventSo saveDataEvent;
    //加载游戏（用于restart按钮加载游戏）
    public VoidEventSo loadDataEvent;
    //将实现ISaveable的类注册进来
    public List<ISaveable> saveables = new List<ISaveable>();

    private Data saveData;

    //获取存储路径
    private string jsonFolder;

    //单例模式
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //Data类没有继承monobehaviour，没有生命周期管理，所以在使用时需要new
        saveData = new Data();
        //获取保存的文件夹
        jsonFolder = Application.persistentDataPath + "/SAVE DATA/";
        //读取保存的问价
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

    //读取键盘上L键，加载保存的数据
    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }



    //如果没有此对象则添加保存
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

        //保存所有实现saveable的类
        foreach (var saveable in saveables)
        {
            saveable.GetSaveData(saveData);
        }

        //扩展名写为什么都可，这里写成.sav
        var resultPath = jsonFolder + "data.sav";
        //将saveData转换为String类型的数据
        var jsonData = JsonConvert.SerializeObject(saveData);
        //判断是否有该保存文件，如果有，则直接写入，如果没有，则创建目录
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        //写文件
        File.WriteAllText(resultPath, jsonData);
    }

    public void Load()
    {
        //这里用foreach报错，是因为saveables被修改了，其原因是在切换场景时人物被启用和关闭了，此时character中会有注册saveables的部分
        foreach (var saveable in saveables)
        {
            saveable.LoadSaveData(saveData);
        }
    }

    private void ReadSaveData() 
    {

        //扩展名写为什么都可，这里写成.sav
        var resultPath = jsonFolder + "data.sav";
        //判断是否有该保存文件，如果有，则直接读，如果没有，则创建目录
        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);
            //将读取的文件反序列化为Data
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }
}
