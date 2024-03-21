using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void RegisterSaveData() 
    {
        //c#9.0可以在接口中写函数的方法体,默认会被执行
        DataManager.instance.RegisterSaveData(this);
    }

    void UnregisterSaveData() => DataManager.instance.UnRegisterSaveData(this);

    void GetSaveData();

    void LoadSaveData();
}
