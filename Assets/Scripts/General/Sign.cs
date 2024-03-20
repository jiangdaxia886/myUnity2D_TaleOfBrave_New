using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    public PlayerInputController playerInputAction;

    public Animator anim;

    public GameObject signSprite;

    public Transform playerTrans;

    private bool canPress;

    private IInteractable targetItem;

    private void Awake()
    {
        //anim = GetComponentInChildren<Animator>();
        //���������Ķ������
        anim = signSprite.GetComponent<Animator>();

        playerInputAction = new PlayerInputController();
        playerInputAction.Enable();
    }

    private void OnEnable()
    {
        //InputSystem.onActionChange += new Action<object, InputActionChange>(OnActionChange);
        //�豸�л�ʱ�򲥷Ų�ͬ����
        InputSystem.onActionChange += OnActionChange;
        //���´򿪱��䰴��
        playerInputAction.GamePlay.Confirm.started += OnConfirm;
    }



    private void OnActionChange(object  obj,InputActionChange actionChange)
    {
        //�������豸�л�һ��ʼʱ��actionChange��ö��ֵ������=ActionStartedʱ��ʾ�豸�л��տ�ʼ��
        if (actionChange == InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;

            switch (d.device)
            {
                case Keyboard:
                    anim.Play("keyBoard");
                    break;
                case XInputController:
                    anim.Play("XBox");
                    break;
            }
        }
    }

    private void Update()
    {
        //�����尴ť��ʾ��ʾ��״̬��canPress�������signSprite��setActive��������canPress����ô��OnActionChangeʱ����anim.Play�ᱨAnimatorδ����
        signSprite.GetComponent<SpriteRenderer>().enabled =  canPress;
        //�������localScale�������player��localScale�����Ե�player��localScale��(-1,1,1)ʱ��������Ҳ��(-1,1,1)����ʱ���������������廹�ǳ���
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            if (targetItem.TriggerAction())
            {
                //�򿪱���ʱ��������
                GetComponent<AudioDefination>()?.PlayAudioClip();
                canPress = false;
            }
        }
    }

    //�����ǩ�ǿɻ�����Interactable���򼤻ť��ʾ
    private void OnTriggerStay2D(Collider2D collision)
    {
        //�뱦�以���걦���tag���Ϊ���ɻ������ʲ�������ʾ�ɻ�����ʶ
        if (collision.CompareTag("Interactable"))
        {
            //Debug.Log("666666");
            canPress = true;
            //��ñ���Ļ����ӿڷ���
            targetItem = collision.GetComponent<IInteractable>();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
