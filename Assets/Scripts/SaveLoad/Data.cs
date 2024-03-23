using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //������sceneloader�и��£�
    public string sceneToSave;
    //�������꣨character�и��£�
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();
    //�������ԣ�character�и��£�
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    //���泡��
    public void SaveGameScene(GameSceneSO savedScene)
    {
        //���л�
        sceneToSave = JsonUtility.ToJson(savedScene);
    }

    //���س����������л���
    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        //var newScene = new GameSceneSO();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}
