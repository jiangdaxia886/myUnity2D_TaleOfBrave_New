using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject newGameButton;

    private void OnEnable()
    {
        //������Ϸ��ť���ݸ���ǰEventSystemѡ�е�GameObject
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    //�˳���Ϸ
    public void ExitGame()
    {
        Debug.Log("quit!!!");
        Application.Quit();
    }
}
