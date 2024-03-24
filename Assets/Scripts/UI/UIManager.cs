using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("事件监听")]
    public CharacterEventSO characterEvent;

    public SceneLoadEventSo sceneUnloadEvent;
    //（用于restart按钮加载游戏）关闭gameover面板
    public VoidEventSo loadDataEvent;
    //（用于character ondie事件）
    public VoidEventSo gameOverEvent;
    //（用于backToMenuEvent按钮加载游戏）关闭gameover面板
    public VoidEventSo backToMenuEvent;

    [Header("组件")]
    public GameObject gameOverPanel;

    public GameObject restartBtn;

    private void OnEnable()
    {
        characterEvent.OnEventRaised += OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent += OnSceneUnloadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        //回到主菜单也关闭面板
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }


    private void OnDisable()
    {
        characterEvent.OnEventRaised -= OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent -= OnSceneUnloadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        //回到主菜单也关闭面板
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }

    //死亡时将gameover面板开启
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        //将重新开始游戏按钮传递给当前EventSystem选中的GameObject
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    //读取游戏数据时将gameover面板关闭
    private void OnLoadDataEvent()
    {
        //Debug.Log("gameOverPanel.SetActive(false);");
        gameOverPanel.SetActive(false);
    }

    //场景加载
    private void OnSceneUnloadEvent(GameSceneSO sceneToLoad, Vector3 PosTogGo, bool fade)
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
