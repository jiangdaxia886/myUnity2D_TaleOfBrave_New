using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//为每个挂载的物体生成GUID用于在实现saveable接口的类中保存
public class DataDefination : MonoBehaviour
{
    public  PersistentType persistentType;

    public string ID;

    private void OnValidate()
    {
        //当保存类型选择为ReadWrite时，生成的GUID不会被修改
        if (persistentType == PersistentType.ReadWrite)
        {
            if (ID == string.Empty)
            {
                ID = System.Guid.NewGuid().ToString();
            }
        }
        else
        { 
            ID = string.Empty;
        }
    }
}
