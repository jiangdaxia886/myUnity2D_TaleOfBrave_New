using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("事件监听")]
    public CharacterEventSO characterEvent;

    public SceneLoadEventSo sceneUnloadEvent;

    private void OnEnable()
    {
        characterEvent.OnEventRaised += OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent += OnSceneLoadEvent;
    }


    private void OnDisable()
    {
        characterEvent.OnEventRaised -= OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent = OnSceneLoadEvent;
    }

    //场景加载
    private void OnSceneLoadEvent(GameSceneSO sceneToLoad, Vector3 PosTogGo, bool fade)
    {
        //在菜单界面则血量不显示
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    //血条变化
    private void OnHealthEvent(Character character)
    {
        var persentage = character.CurrentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
        playerStatBar.OnPowerChange(character);
    }
}
