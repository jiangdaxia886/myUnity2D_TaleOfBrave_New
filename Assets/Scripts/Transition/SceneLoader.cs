using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,ISaveable
{
    public Transform playerTrans;

    public Vector3 firstPosition;

    public Vector3 menuPosition;

    [Header("�¼�����")]
    public SceneLoadEventSo loadEventSo;

    public VoidEventSo newGameEvent;

    public VoidEventSo backToMenuEvent;

    [Header("�㲥")]
    public VoidEventSo afterSceneLoadedEvent;

    public FadeEventSO fadeEvent;

    public SceneLoadEventSo sceneUnloadEvent;

    [Header("����")]
    public GameSceneSO MenuScene;

    public GameSceneSO firstLoadScene;

    private GameSceneSO currentLoadScene;

    private GameSceneSO sceneToLoad;

    private Vector3 positionToGo;

    private bool fadeScreen;

    private bool isLoading;

    public float fadeDuration;
    private void Awake()
    {
        //�첽��������
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);


    }

    private void Start()
    {
        //NewGame();
        //��Ϸ��ʼ����menu����
        sceneToLoad = MenuScene;
        loadEventSo.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    //��Enableʱ���˷���ע�ᵽ�¼��У�������ô��¼���invoke��ֱ�ӵ���OnLoadRequestEvent����
    private void OnEnable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSo.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.UnregisterSaveData();
    }

    private void OnBackToMenuEvent()
    {
        sceneToLoad = MenuScene;
        loadEventSo.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    private void NewGame()
    {
        //��Ϸ��ʼ���ص�һ������
        Debug.Log("NewGame:::::");
        sceneToLoad = firstLoadScene;
        
        loadEventSo.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    //�������ط���
    private void OnLoadRequestEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = arg0;
        positionToGo = arg1;
        fadeScreen = arg2;

        //Debug.Log(sceneToLoad.sceneReference.SubObjectName);
        //��currentLoadScene��Ϊ�գ����л���һ�γ�������OnLoadCompleted���������и�ֵ����ж�س������������³���
        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        //��û���л���������currentLoadSceneΪ��ʱ��ֱ�Ӽ����³���
        else
        {
            loadNewScene();
        }

    }

    //ж�س����������³���Э��
    private IEnumerator UnLoadPreviousScene()
    {
        //ж�س���֮ǰ�ر�����ͼƬ
        playerTrans.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //ж�س����𽥱��
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
            //Debug.Log("ж�س���");
        }
        //yield��ʾ�ȴ����ȴ�WaitForSecondsʱ�������ĵȴ��첽����ж��
        yield return new WaitForSeconds(fadeDuration);

        //ִ����ж�س���Я��֮����ִ��sceneUnloadEvent����Ѫ��
        //sceneUnloadEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

        if (currentLoadScene != null)
        {
            //ж�س���
            yield return currentLoadScene.sceneReference.UnLoadScene();
        }
        
        //Debug.Log("ж���������");
        loadNewScene();
    }

    //���س���
    private void loadNewScene()
    {
        //���һ��ʼȡ����ѡ���menu���滹���ܶ����������ﲻ��һ��ʼȡ����ѡ
        //�ر�����������������ô������inputcontroller.gameobject.disable()������Ч,��ԭ������������gameObject��Ч����OnLoadCompleted����������gameObject��Ч��������������Ч���ִ��onEnable������playerController�е�onEnable��������inputController.Enable(); ����inputcontroller��Ч
        // playerTrans.gameObject.SetActive(false);
        //�첽���س����������첽�Ǻ�����ж�س�����ͬ�����еģ����³�����ûж���꣬��������ƶ���λ��
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        //�������غ�ִ��OnLoadCompleted
        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// �������غú�
    /// </summary>
    /// <param name="handle"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        //�л���ǰ����
        currentLoadScene = sceneToLoad;
        //�ı�����λ��
        playerTrans.transform.position = positionToGo;
        //��������ͼƬ
        playerTrans.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        //���س������͸��
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        //������صĳ�����location������menu������������ƶ�
        if (currentLoadScene.sceneType == SceneType.Location)
        {
            //Debug.Log("�������");
            //����������ɺ��¼�(Ŀǰ�ǻ�ȡ����߽硢�������)
            afterSceneLoadedEvent?.RaiseEvent();
        }
        //����������ɺ���ִ��sceneUnloadEvent����Ѫ��
        sceneUnloadEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadSaveData(Data data)
    {
        var playerID = playerTrans.GetComponent<Character>().GetDataID().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            //�������
            positionToGo = data.characterPosDict[playerID].ToVector3();
            //��ó���
            sceneToLoad = data.GetSavedScene();
            loadEventSo.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
