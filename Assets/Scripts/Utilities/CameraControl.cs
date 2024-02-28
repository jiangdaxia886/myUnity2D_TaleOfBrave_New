using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;

    public CinemachineImpulseSource impulseSource;

    public VoidEventSo cameraShakeEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    //受伤时事件执行
    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }

    //摄像机震动
    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void Start()
    {
        GetNewCameraBounds();
    }

    private void GetNewCameraBounds() 
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        //获得新场景的摄像机边界
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        //清除缓存
        confiner2D.InvalidateCache();
    }
}
