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

    public Vector3 menuPosition;

    [Header("事件监听")]
    public SceneLoadEventSo loadEventSo;

    public VoidEventSo newGameEvent;

    [Header("广播")]
    public VoidEventSo afterSceneLoadedEvent;

    public FadeEventSO fadeEvent;

    public SceneLoadEventSo sceneUnloadEvent;

    [Header("场景")]
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
        //异步场景加载
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadScene = firstLoadScene;
        //currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);


    }

    private void Start()
    {
        //NewGame();
        //游戏开始加载menu场景
        sceneToLoad = MenuScene;
        loadEventSo.RaiseLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    //在Enable时将此方法注册到事件中，后面调用此事件的invoke则直接调用OnLoadRequestEvent方法
    private void OnEnable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        //Debug.Log("Enable!!!!!!!!!");
    }

    private void OnDisable()
    {
        loadEventSo.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        //游戏开始加载第一个场景
        sceneToLoad = firstLoadScene;
        loadEventSo.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    //场景加载方法
    private void OnLoadRequestEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = arg0;
        positionToGo = arg1;
        fadeScreen = arg2;

        //Debug.Log(sceneToLoad.sceneReference.SubObjectName);
        //当currentLoadScene不为空（即切换过一次场景后在OnLoadCompleted（）方法中赋值），卸载场景，并加载新场景
        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        //当没有切换过场景（currentLoadScene为空时）直接加载新场景
        else
        {
            loadNewScene();
        }

    }

    //卸载场景、加载新场景协程
    private IEnumerator UnLoadPreviousScene()
    {
        //卸载场景逐渐变黑
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        //yield表示等待，等待WaitForSeconds时间或下面的等待异步场景卸载
        yield return new WaitForSeconds(fadeDuration);

        //执行完卸载场景携程之后再执行sceneUnloadEvent加载血条
        //sceneUnloadEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

        if (currentLoadScene != null)
        {
            //卸载场景
            yield return currentLoadScene.sceneReference.UnLoadScene();
        }
        //卸载场景之后关闭人物
        playerTrans.gameObject.SetActive(false);
        loadNewScene();
    }

    //加载场景
    private void loadNewScene()
    {
        //关闭人物如果放在这里，那么让人物inputcontroller.gameobject.disable()方法无效,其原因是这里是让gameObject无效，在OnLoadCompleted方法中又让gameObject有效，于是inputcontroller有效
        // playerTrans.gameObject.SetActive(false);
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        //场景加载好执行OnLoadCompleted
        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// 场景加载好后
    /// </summary>
    /// <param name="handle"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        //切换当前场景
        currentLoadScene = sceneToLoad;
        //改变人物位置
        playerTrans.transform.position = positionToGo;
        //启用人物
        playerTrans.gameObject.SetActive(true);
        //加载场景后变透明
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        //如果加载的场景是location而不是menu，则人物可以移动
        if (currentLoadScene.sceneType == SceneType.Location)
        {
            //Debug.Log("解除控制");
            //场景加载完成后事件(目前是获取相机边界、人物解锁)
            afterSceneLoadedEvent?.RaiseEvent();
        }
        //场景加载完成后再执行sceneUnloadEvent加载血条
        sceneUnloadEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);
    }
}
