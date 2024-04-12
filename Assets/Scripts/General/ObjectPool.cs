using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;

    //Ԥ����
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    //Ԥ�������صĸ�����أ�Ϊ�˷�ֹhierarchy����̫��Ԥ����̫���ң�����һ��������أ�
    private GameObject pool;

    public static ObjectPool Instance {
        get
        {
            if (instance == null)
            { 
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    /**
     * ����Ԥ�����ö�����и�Ԥ����Ķ���
     * ����������򴴽�һ��������
     */
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _obj;
        //�鿴��������Ƿ���ڸ�Ԥ����
        //��������ڸ�Ԥ������Ԥ����ʵ�����Ķ������Ϊ0
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        { 
            //ʵ������Ԥ���岢��������
            _obj = GameObject.Instantiate(prefab);
            PushObject(_obj);
            //���������ز������򴴽�һ��
            if(pool == null)
                pool = new GameObject("ObjectPool");
            //�鿴�Ƿ��ж�ӦԤ������Ӷ����
            GameObject child = GameObject.Find(prefab.name);
            //�������򴴽��Ӷ����
            if (!child)
            {
                child = new GameObject(prefab.name);
                //���Ӷ������Ϊ������ص�������
                child.transform.SetParent(pool.transform);
            }
            //��Ԥ������Ϊ�Ӷ���ص�������
            _obj.transform.SetParent(child.transform);
        }
        //��Ԥ����Ӷ�����г��в�����
        _obj = objectPool[prefab.name].Dequeue();
        _obj.SetActive(true);
        return _obj;
    }


    /**
     * �������Ԥ����Żض����
     * ������Ԥ����ʧЧ
     */
    public void PushObject(GameObject prefab)
    {
        //��Ԥ����������е�(Clone)ȥ����ʹ��Instantiate���ɵ��������ְ���Clone��
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        //�鿴��������Ƿ���ڸ�Ԥ������Ӷ����
        //�����������
        if (!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>());
        }
        //������������Ӷ���صĶ���
        objectPool[_name].Enqueue(prefab);
        //ȡ������
        prefab.SetActive(false);
    }
}
