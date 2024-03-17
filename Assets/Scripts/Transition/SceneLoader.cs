using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;

    public Vector3 firstPosition;

    [Header("�¼�����")]
    public SceneLoadEventSo loadEventSo;

    public GameSceneSO firstLoadScene;

    [Header("�㲥")]
    public VoidEventSo afterSceneLoadedEvent;

    public FadeEventSO fadeEvent;

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
        NewGame();
    }

    //��Enableʱ���˷���ע�ᵽ�¼��У�������ô��¼���invoke��ֱ�ӵ���OnLoadRequestEvent����
    private void OnEnable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
        //Debug.Log("Enable!!!!!!!!!");
    }

    private void OnDisable()
    {
        loadEventSo.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void NewGame()
    {
        //��Ϸ��ʼ���ص�һ������
        sceneToLoad = firstLoadScene;
        loadEventSo.RaiseLoadRequestEvent(sceneToLoad, firstPosition,true);
        OnLoadRequestEvent(sceneToLoad, firstPosition, true);
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
        //ж�س����𽥱��
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        //yield��ʾ�ȴ����ȴ�WaitForSecondsʱ�������ĵȴ��첽����ж��
        yield return new WaitForSeconds(fadeDuration);
        if(currentLoadScene != null)
        {
            //ж�س���
            yield return currentLoadScene.sceneReference.UnLoadScene();
        }
        
        loadNewScene();
    }

    //���س���
    private void loadNewScene() 
    {
        //���س���֮ǰ�ر�����
        playerTrans.gameObject.SetActive(false);
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
        //��������
        playerTrans.gameObject.SetActive(true);
        //���س������͸��
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        //������صĳ�����location������menu������������ƶ�
        if (currentLoadScene.sceneType == SceneType.Location)
        {
            //����������ɺ��¼�(Ŀǰ�ǻ�ȡ����߽硢�������)
            afterSceneLoadedEvent?.RaiseEvent();
        }
    }
}
