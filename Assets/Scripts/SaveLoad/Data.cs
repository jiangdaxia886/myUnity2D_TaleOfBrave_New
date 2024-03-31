using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //������sceneloader�и��£�
    public string sceneToSave;
    //�������꣨character�и��£�
    public Dictionary<string, SerializeVector3> characterPosDict = new Dictionary<string, SerializeVector3>();
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

//vector3���ܱ����л�������float���Ա����л���������һ�������float�������洢λ��
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
