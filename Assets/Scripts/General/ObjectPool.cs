using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;

    //预制体
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    //预制体对象池的父对象池（为了防止hierarchy窗口太多预制体太杂乱，创建一个父对象池）
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
     * 传入预制体获得对象池中该预制体的对象
     * 如果不存在则创建一个并返回
     */
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _obj;
        //查看对象池中是否存在该预制体
        //如果不存在该预制体或该预制体实例化的对象个数为0
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        { 
            //实例化该预制体并放入对象池
            _obj = GameObject.Instantiate(prefab);
            PushObject(_obj);
            //如果父对象池不存在则创建一个
            if(pool == null)
                pool = new GameObject("ObjectPool");
            //查看是否有对应预制体的子对象池
            GameObject child = GameObject.Find(prefab.name);
            //不存在则创建子对象池
            if (!child)
            {
                child = new GameObject(prefab.name);
                //将子对象池设为父对象池的子物体
                child.transform.SetParent(pool.transform);
            }
            //将预制体设为子对象池的子物体
            _obj.transform.SetParent(child.transform);
        }
        //将预制体从对象池中出列并返回
        _obj = objectPool[prefab.name].Dequeue();
        _obj.SetActive(true);
        return _obj;
    }


    /**
     * 将传入的预制体放回对象池
     * 并将该预制体失效
     */
    public void PushObject(GameObject prefab)
    {
        //将预制体的名字中的(Clone)去掉（使用Instantiate生成的物体名字包含Clone）
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        //查看对象池中是否存在该预制体的子对象池
        //不存在则添加
        if (!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>());
        }
        //将该物体放入子对象池的队列
        objectPool[_name].Enqueue(prefab);
        //取消激活
        prefab.SetActive(false);
    }
}
