using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [Header("���")]
    public GameObject gameOverPanel;

    public GameObject restartBtn;

    private void OnEnable()
    {
        characterEvent.OnEventRaised += OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent += OnSceneUnloadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        //�ص����˵�Ҳ�ر����
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }


    private void OnDisable()
    {
        characterEvent.OnEventRaised -= OnHealthEvent;
        sceneUnloadEvent.LoadRequestEvent -= OnSceneUnloadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        //�ص����˵�Ҳ�ر����
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
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
