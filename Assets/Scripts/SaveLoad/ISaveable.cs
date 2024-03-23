using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefination GetDataID();

    void RegisterSaveData() 
    {
        //c#9.0�����ڽӿ���д�����ķ�����,Ĭ�ϻᱻִ��
        //c#�в���ͨ��ʵ�����ʾ�̬������ֻ��ͨ����������
        //DataManager.instance.instance = new DataManager(); ��
        //DataManager.instance = new DataManager();����
        DataManager.instance.RegisterSaveData(this);
        
    }

    void UnregisterSaveData() => DataManager.instance.UnRegisterSaveData(this);

    void GetSaveData(Data data);

    void LoadSaveData(Data data);
}
