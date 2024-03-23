using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("广播")]
    public VoidEventSo saveDataEvent;

    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;

    public GameObject lightObj;

    public Sprite darkSprite;

    public Sprite lightSprite;

    public bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        lightObj.SetActive(isDone);
    }

    public bool TriggerAction()
    {
        //未保存则保存
        if (!isDone)
        {
            //变亮
            spriteRenderer.sprite = lightSprite;
            //不可互动
            this.gameObject.tag = "Untagged";
            isDone = true;
            //点光源
            lightObj.SetActive(isDone);
            //保存数据
            saveDataEvent.RaiseEvent();
            return true;
        }
        return false;

    }



   
}
