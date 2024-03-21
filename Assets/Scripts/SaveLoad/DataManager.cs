using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //��̬������Ϸһ��ʼ�ͻ�洢���ڴ���
    public static DataManager instance;

    //����ģʽ
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void RegisterSaveData(ISaveable saveable)
    { 
        
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {

    }
}
