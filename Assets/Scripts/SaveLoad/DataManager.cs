using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //静态对象，游戏一开始就会存储在内存中
    public static DataManager instance;

    //单例模式
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
