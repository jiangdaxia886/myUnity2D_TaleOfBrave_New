using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour,IInteractable
{
    public SceneLoadEventSo loadEventSo;

    public GameSceneSO sceneToGo;
    //��Ҫ���͵�������
    public Vector3 postitionToGo;
    public bool TriggerAction()
    {
        //Debug.Log("����");
        loadEventSo.RaiseLoadRequestEvent(sceneToGo, postitionToGo, true);
        return true;
    }

}
