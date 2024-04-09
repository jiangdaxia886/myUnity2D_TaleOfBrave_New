using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    //子弹时间间隔
    public float interval;
    //子弹
    public GameObject bulletPrefab;
    //发射子弹的枪口位置
    public Vector2 gunEnd;
    //子弹射击方向
    public Vector2 direction;
    //计时器
    public float timer = 0;

    private Animator anim;

    private PlayerController playerController;

    //用unity自动生成的PlayerInputController控制器类
    public PlayerInputController inputController;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputController = new PlayerInputController();
        inputController.Enable();
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
        // Start is called before the first frame update
        void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        direction = new Vector2( transform.localScale.x,0);
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
                timer = 0;
        }
        //按下射击键
        else if(inputController.GamePlay.Fire.WasPressedThisFrame())
        {
            Fire();
            playerController.isAttack = true;
            timer = interval;
        }
    }

    private void Fire()
    {
        anim.SetTrigger("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, (Vector2)this.gameObject.transform.position + gunEnd, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetSpeed(direction);
    }
}
