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
        //获得子物体的动画组件
        anim = signSprite.GetComponent<Animator>();

        playerInputAction = new PlayerInputController();
        playerInputAction.Enable();
    }

    private void OnEnable()
    {
        //InputSystem.onActionChange += new Action<object, InputActionChange>(OnActionChange);
        //设备切换时则播放不同动画
        InputSystem.onActionChange += OnActionChange;
        //按下打开宝箱按键
        playerInputAction.GamePlay.Confirm.started += OnConfirm;
    }



    private void OnActionChange(object  obj,InputActionChange actionChange)
    {
        //当输入设备切换一开始时（actionChange是枚举值，当其=ActionStarted时表示设备切换刚开始）
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
        //子物体按钮提示显示的状态是canPress，如果让signSprite的setActive方法等于canPress，那么在OnActionChange时运行anim.Play会报Animator未激活
        signSprite.GetComponent<SpriteRenderer>().enabled =  canPress;
        //子物体的localScale是相对于player的localScale，所以当player的localScale是(-1,1,1)时，子物体也是(-1,1,1)，此时负负得正，子物体还是朝右
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            if (targetItem.TriggerAction())
            {
                //打开宝箱时播放音乐
                GetComponent<AudioDefination>()?.PlayAudioClip();
                canPress = false;
            }
        }
    }

    //如果标签是可互动的Interactable，则激活按钮提示
    private void OnTriggerStay2D(Collider2D collision)
    {
        //与宝箱互动完宝箱的tag会改为不可互动，故不会再显示可互动标识
        if (collision.CompareTag("Interactable"))
        {
            //Debug.Log("666666");
            canPress = true;
            //获得宝箱的互动接口方法
            targetItem = collision.GetComponent<IInteractable>();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
