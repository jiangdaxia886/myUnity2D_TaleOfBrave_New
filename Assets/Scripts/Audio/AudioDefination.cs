using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSO playAudioEvent;

    public AudioClip audioClip;

    public bool playOnEnable;

    //当本物体生效时播放音乐
    private void OnEnable()
    {
        if (playOnEnable) 
        {
            PlayAudioClip();
        }
    }

    public void PlayAudioClip()
    {
        playAudioEvent.RaiseEvent(audioClip);
    }
}
