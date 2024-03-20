using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
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
        //Œ¥±£¥Ê‘Ú±£¥Ê
        if (!isDone)
        {
            spriteRenderer.sprite = lightSprite;
            this.gameObject.tag = "Untagged";
            isDone = true;
            lightObj.SetActive(isDone);
            return true;
        }
        return false;

    }



   
}
