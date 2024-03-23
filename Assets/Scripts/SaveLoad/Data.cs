using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //场景（sceneloader中更新）
    public string sceneToSave;
    //人物坐标（character中更新）
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();
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
