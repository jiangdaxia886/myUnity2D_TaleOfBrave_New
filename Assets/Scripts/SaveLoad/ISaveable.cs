using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefination GetDataID();

    void RegisterSaveData() 
    {
        //c#9.0可以在接口中写函数的方法体,默认会被执行
        //c#中不能通过实例访问静态变量，只能通过类名访问
        //DataManager.instance.instance = new DataManager(); 错
        //DataManager.instance = new DataManager();可以
        DataManager.instance.RegisterSaveData(this);
        
    }

    void UnregisterSaveData() => DataManager.instance.UnRegisterSaveData(this);

    void GetSaveData(Data data);

    void LoadSaveData(Data data);
}
