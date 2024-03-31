using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //场景（sceneloader中更新）
    public string sceneToSave;
    //人物坐标（character中更新）
    public Dictionary<string, SerializeVector3> characterPosDict = new Dictionary<string, SerializeVector3>();
    //人物属性（character中更新）
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    //保存场景
    public void SaveGameScene(GameSceneSO savedScene)
    {
        //序列化
        sceneToSave = JsonUtility.ToJson(savedScene);
    }

    //加载场景（反序列化）
    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        //var newScene = new GameSceneSO();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}

//vector3不能被序列化，但是float可以被序列化，所以用一个对象的float变量来存储位置
public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    { 
        return new Vector3(x, y, z);
    }
}
