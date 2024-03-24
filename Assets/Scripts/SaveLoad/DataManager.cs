using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //单例模式
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //Data类没有继承monobehaviour，没有生命周期管理，所以在使用时需要new
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

        /*foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + ":::" + item.Value);
        }*/
    }

    public void Load()
    {
        //这里用foreach报错，是因为saveables被修改了，其原因是在切换场景时人物被启用和关闭了，此时character中会有注册saveables的部分
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
