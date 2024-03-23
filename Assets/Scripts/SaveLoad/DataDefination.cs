using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ϊÿ�����ص���������GUID������ʵ��saveable�ӿڵ����б���
public class DataDefination : MonoBehaviour
{
    public  PersistentType persistentType;

    public string ID;

    private void OnValidate()
    {
        //����������ѡ��ΪReadWriteʱ�����ɵ�GUID���ᱻ�޸�
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
