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

    [Header("事件监听")]
    public SceneLoadEventSo loadEventSo;

    public GameSceneSO firstLoadScene;

    [Header("广播")]
    public VoidEventSo afterSceneLoadedEvent;

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
        NewGame();
    }

    //在Enable时将此方法注册到事件中，后面调用此事件的invoke则直接调用OnLoadRequestEvent方法
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
        //游戏开始加载第一个场景
        sceneToLoad = firstLoadScene;
        OnLoadRequestEvent(sceneToLoad, firstPosition, true);
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
        if (fadeScreen)
        { 
        
        }
        //yield表示等待，等待WaitForSeconds时间或下面的等待异步场景卸载
        yield return new WaitForSeconds(fadeDuration);
        if(currentLoadScene != null)
        {
            //卸载场景
            yield return currentLoadScene.sceneReference.UnLoadScene();
        }
        //卸载场景后关闭人物
        playerTrans.gameObject.SetActive(false);
        loadNewScene();
    }

    //加载场景
    private void loadNewScene() 
    {
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
        isLoading = false;

        //场景加载完成后事件
        afterSceneLoadedEvent?.RaiseEvent();
    }
}
