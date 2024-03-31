using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    //将AudioManager传递来的音量同步到音量滑块
    public FloatEventSO syncVolumeEvent;

    [Header("广播")]
    public VoidEventSo pauseEvent;

    [Header("组件")]
    public GameObject gameOverPanel;

    public GameObject restartBtn;

    public GameObject mobileTouch;

    //设置按钮
    public Button settingsBtn;
    //暂停界面
    public GameObject pausePanel;

    //音量滑块
    public Slider volumeSlider;

    //如果是主机那么触屏按钮不显示
    private void Awake()
    {
#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif
        //点击齿轮按钮执行TogglePausePanel
        settingsBtn.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable()
    {
        characterEvent.OnEventRaised += OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent += OnSceneUnloadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        //回到主菜单也关闭面板
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        //将AudioManager传递来的音量同步到音量滑块
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }


    private void OnDisable()
    {
        characterEvent.OnEventRaised -= OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent -= OnSceneUnloadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }

    //调整音量滑块
    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    //暂停面板开关
    private void TogglePausePanel()
    {
        //在Hierarchy暂停界面是激活的,则按下settings按钮关闭界面，游戏恢复常速
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            //游戏速度正常
            Time.timeScale = 1.0f;
        }
        //游戏暂停
        else 
        {
            //暂停时，通知AudioManager将音量传递给syncVolumeEvent事件
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            //游戏速度停止
            Time.timeScale = 0;
        }
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
