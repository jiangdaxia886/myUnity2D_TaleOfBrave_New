using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("�¼�����")]
    public SceneLoadEventSo loadEventSo;

    public GameSceneSO firstLoadScene;

    private GameSceneSO currentLoadScene;

    private GameSceneSO sceneToLoad;

    private Vector3 positionToGo;

    private bool fadeScreen;

    public float fadeDuration;
    private void Awake()
    {
        //�첽��������
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
        //ж�س������������³���
        StartCoroutine(UnLoadPreviousScene());
    }

    //ж�س���Э��
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        { 
        
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
        sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
    }
}
