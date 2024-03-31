using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("�¼�����")]
    public CharacterEventSO characterEvent;

    public SceneLoadEventSo sceneUnloadEvent;
    //������restart��ť������Ϸ���ر�gameover���
    public VoidEventSo loadDataEvent;
    //������character ondie�¼���
    public VoidEventSo gameOverEvent;
    //������backToMenuEvent��ť������Ϸ���ر�gameover���
    public VoidEventSo backToMenuEvent;
    //��AudioManager������������ͬ������������
    public FloatEventSO syncVolumeEvent;

    [Header("�㲥")]
    public VoidEventSo pauseEvent;

    [Header("���")]
    public GameObject gameOverPanel;

    public GameObject restartBtn;

    public GameObject mobileTouch;

    //���ð�ť
    public Button settingsBtn;
    //��ͣ����
    public GameObject pausePanel;

    //��������
    public Slider volumeSlider;

    //�����������ô������ť����ʾ
    private void Awake()
    {
#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif
        //������ְ�ťִ��TogglePausePanel
        settingsBtn.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable()
    {
        characterEvent.OnEventRaised += OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent += OnSceneUnloadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        //�ص����˵�Ҳ�ر����
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        //��AudioManager������������ͬ������������
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

    //������������
    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    //��ͣ��忪��
    private void TogglePausePanel()
    {
        //��Hierarchy��ͣ�����Ǽ����,����settings��ť�رս��棬��Ϸ�ָ�����
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            //��Ϸ�ٶ�����
            Time.timeScale = 1.0f;
        }
        //��Ϸ��ͣ
        else 
        {
            //��ͣʱ��֪ͨAudioManager���������ݸ�syncVolumeEvent�¼�
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            //��Ϸ�ٶ�ֹͣ
            Time.timeScale = 0;
        }
    }

    //����ʱ��gameover��忪��
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        //�����¿�ʼ��Ϸ��ť���ݸ���ǰEventSystemѡ�е�GameObject
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    //��ȡ��Ϸ����ʱ��gameover���ر�
    private void OnLoadDataEvent()
    {
        //Debug.Log("gameOverPanel.SetActive(false);");
        gameOverPanel.SetActive(false);
    }

    //��������
    private void OnSceneUnloadEvent(GameSceneSO sceneToLoad, Vector3 PosTogGo, bool fade)
    {
        //�ڲ˵�������Ѫ������ʾ
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    //Ѫ���仯
    private void OnHealthEvent(Character character)
    {
        var persentage = character.CurrentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
        playerStatBar.OnPowerChange(character);
    }
}
