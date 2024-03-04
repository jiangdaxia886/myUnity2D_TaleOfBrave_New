using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("�¼�����")]
    public PlayAudioEventSO FXEvent;

    public PlayAudioEventSO BGMEvent;

    public PlayAudioEventSO HurtEvent;

    public AudioSource BGMSource;

    public AudioSource FXSource;

    public AudioSource HurtSource;

    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        HurtEvent.OnEventRaised += OnHurtEvent;
    }


    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        HurtEvent.OnEventRaised -= OnHurtEvent;
    }


    private void OnHurtEvent(AudioClip clip)
    {
        HurtSource.clip = clip;
        HurtSource.Play();
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    //ʹ����ԴFXSource������Ч
    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
}
