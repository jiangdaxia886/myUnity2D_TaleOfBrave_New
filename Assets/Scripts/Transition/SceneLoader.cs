using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("事件监听")]
    public SceneLoadEventSo loadEventSo;

    public GameSceneSO firstLoadScene;

    private GameSceneSO currentLoadScene;

    private GameSceneSO sceneToLoad;

    private Vector3 positionToGo;

    private bool fadeScreen;

    public float fadeDuration;
    private void Awake()
    {
        //异步场景加载
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        currentLoadScene = firstLoadScene;
        currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        loadEventSo.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSo.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        sceneToLoad = arg0;
        positionToGo = arg1;
        fadeScreen = arg2;

        //Debug.Log(sceneToLoad.sceneReference.SubObjectName);
        //卸载场景，并加载新场景
        StartCoroutine(UnLoadPreviousScene());
    }

    //卸载场景协程
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
        loadNewScene();
    }

    //加载场景
    private void loadNewScene() 
    {
        sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
