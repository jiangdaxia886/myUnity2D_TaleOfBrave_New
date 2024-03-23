using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("�㲥")]
    public VoidEventSo saveDataEvent;

    [Header("��������")]
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
        //δ�����򱣴�
        if (!isDone)
        {
            //����
            spriteRenderer.sprite = lightSprite;
            //���ɻ���
            this.gameObject.tag = "Untagged";
            isDone = true;
            //���Դ
            lightObj.SetActive(isDone);
            //��������
            saveDataEvent.RaiseEvent();
            return true;
        }
        return false;

    }



   
}
