using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class FadeCanvas : MonoBehaviour
{
    [Header("�¼�����")]
    public FadeEventSO fadeEvent;

    public Image fadeImage;

    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
        //Debug.Log("fadeEvent  OnEnable");
    }

    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }

    private void OnFadeEvent(Color target,float duration, bool fadeIn)
    {
        //��ΪĿ����ɫtarget
        fadeImage.DOBlendableColor(target, duration);
        //Debug.Log("OnFadeEvent:��ΪĿ����ɫtarget");
    }
}
