using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("�¼�����")]
    public VoidEventSo afterSceneLoadedEvent;

    private CinemachineConfiner2D confiner2D;

    public CinemachineImpulseSource impulseSource;

    public VoidEventSo cameraShakeEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    //����ʱ�¼�ִ��
    //��Enableʱ���˷���ע�ᵽ�¼��У�������ô��¼���invoke��ֱ�ӵ���OnCameraShakeEvent��OnAfterSceneLoadedEvent����
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

    //�������
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
        //����³�����������߽�
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        //�������
        confiner2D.InvalidateCache();
    }
}
