using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("事件监听")]
    public PlayAudioEventSO FXEvent;

    public PlayAudioEventSO BGMEvent;

    public PlayAudioEventSO HurtEvent;
    //调整音量
    public FloatEventSO VolumeChangeEvent;
    //监听暂停
    public VoidEventSo pauseEvent;

    [Header("广播")]
    public FloatEventSO syncVolumeEvent;

    [Header("组件")]
    public AudioSource BGMSource;

    public AudioSource FXSource;

    public AudioSource HurtSource;

    public AudioMixer mixer;

    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        HurtEvent.OnEventRaised += OnHurtEvent;
        VolumeChangeEvent.OnEventRaised += OnVolumeChangeEvent;
        pauseEvent.OnEventRaised += OnPauseEvent;
    }


    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        HurtEvent.OnEventRaised -= OnHurtEvent;
        VolumeChangeEvent.OnEventRaised -= OnVolumeChangeEvent;
        pauseEvent.OnEventRaised -= OnPauseEvent;
    }


    private void OnPauseEvent()
    {
        float amount;
        mixer.GetFloat("MasterVolume", out amount);
        syncVolumeEvent.RaiseEvent(amount);
        //Debug.Log("OnPauseEvent:amount" + amount);
    }

    private void OnVolumeChangeEvent(float amount)
    {
        //mixer的音量是-80~20分贝
        //MasterVolume是暴露出来的mixer的参数名
        //Debug.Log("OnVolumeChangeEvent:amount" + amount);
        mixer.SetFloat("MasterVolume", amount * 100 - 80);
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
        //Debug.Log("play BGM");
    }

    //使用声源FXSource播放音效
    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
}
