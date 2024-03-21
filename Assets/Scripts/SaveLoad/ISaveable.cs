using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void RegisterSaveData() 
    {
        //c#9.0�����ڽӿ���д�����ķ�����,Ĭ�ϻᱻִ��
        DataManager.instance.RegisterSaveData(this);
    }

    void UnregisterSaveData() => DataManager.instance.UnRegisterSaveData(this);

    void GetSaveData();

    void LoadSaveData();
}
