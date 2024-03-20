using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;

    public Sprite openSprite;

    public Sprite closeSprite;

    //当后面通过其他场景进来之后的宝箱状态，如果被打开了则不能互动（待完成）
    public bool isDone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //当场景被加载时物体此时被启动，显示宝箱对应的图片
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }

    //打开宝箱
    public bool TriggerAction()
    {
        //Debug.Log("open chest");
        if (!isDone)
        { 
            openChest();
            return true;
        }
        return false;
    }


    private void openChest()
    {
        //将宝箱的图片改为打开
        spriteRenderer.sprite = openSprite;
        isDone = true;
        //宝箱被打开后修改标签为untagged，互动动画就不会显示
        this.gameObject.tag = "Untagged";
    }
}
