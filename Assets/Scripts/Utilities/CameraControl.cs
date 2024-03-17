using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSo afterSceneLoadedEvent;

    private CinemachineConfiner2D confiner2D;

    public CinemachineImpulseSource impulseSource;

    public VoidEventSo cameraShakeEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    //受伤时事件执行
    //在Enable时将此方法注册到事件中，后面调用此事件的invoke则直接调用OnCameraShakeEvent、OnAfterSceneLoadedEvent方法
    private void OnEnable()
    {
        //Debug.Log("find bounds!!!!!!!OnEnable");
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        GetNewCameraBounds();
    }

    //摄像机震动
    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

/*    private void Start()
    {
        GetNewCameraBounds();
    }*/

    private void GetNewCameraBounds() 
    {
        Debug.Log("find bounds!!!!!!!");
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        Debug.Log("find bounds!!!!!!!");
        //获得新场景的摄像机边界
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        //清除缓存
        confiner2D.InvalidateCache();
    }
}
