using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("�¼�����")]
    public CharacterEventSO characterEvent;

    public SceneLoadEventSo sceneLoadEvent;

    private void OnEnable()
    {
        characterEvent.OnEventRaised += OnHealthEvent;
        sceneLoadEvent.LoadRequestEvent += OnSceneLoadEvent;
    }


    private void OnDisable()
    {
        characterEvent.OnEventRaised -= OnHealthEvent;
        sceneLoadEvent.LoadRequestEvent = OnSceneLoadEvent;
    }

    private void OnSceneLoadEvent(GameSceneSO sceneToLoad, Vector3 PosTogGo, bool fade)
    {
        //�ڲ˵�������Ѫ������ʾ
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.CurrentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
        playerStatBar.OnPowerChange(character);
    }
}
