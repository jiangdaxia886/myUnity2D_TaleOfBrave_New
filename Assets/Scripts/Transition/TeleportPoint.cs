using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour,IInteractable
{
    public SceneLoadEventSo loadEventSo;

    public GameSceneSO sceneToGo;
    //需要传送到的坐标
    public Vector3 postitionToGo;
    public bool TriggerAction()
    {
        //Debug.Log("传送");
        loadEventSo.RaiseLoadRequestEvent(sceneToGo, postitionToGo, true);
        return true;
    }

}
