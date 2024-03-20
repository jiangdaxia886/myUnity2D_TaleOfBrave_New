using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;

    public Sprite openSprite;

    public Sprite closeSprite;

    //������ͨ��������������֮��ı���״̬��������������ܻ���������ɣ�
    public bool isDone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //������������ʱ�����ʱ����������ʾ�����Ӧ��ͼƬ
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }

    //�򿪱���
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
        //�������ͼƬ��Ϊ��
        spriteRenderer.sprite = openSprite;
        isDone = true;
        //���䱻�򿪺��޸ı�ǩΪuntagged�����������Ͳ�����ʾ
        this.gameObject.tag = "Untagged";
    }
}
