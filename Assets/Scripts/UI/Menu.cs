using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject newGameButton;

    private void OnEnable()
    {
        //将新游戏按钮传递给当前EventSystem选中的GameObject
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    //退出游戏
    public void ExitGame()
    {
        Debug.Log("quit!!!");
        Application.Quit();
    }
}
