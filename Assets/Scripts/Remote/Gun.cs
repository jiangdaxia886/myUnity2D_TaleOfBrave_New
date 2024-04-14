using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Gun : MonoBehaviour
{
    //子弹时间间隔
    public float interval;
    //子弹
    public GameObject bulletPrefab;
    //子弹射击方向
    public Vector2 direction;
    //计时器
    public float timer = 0;

    public int firePowerCost;

    private Animator anim;

    private PlayerController playerController;

    private Character character;

    public GameObject player;

    //用unity自动生成的PlayerInputController控制器类
    public PlayerInputController inputController;


    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        character = player.GetComponent<Character>();
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
        anim = player.GetComponent<Animator>();
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
        //按下射击键且射击能量够用
        else if(inputController.GamePlay.Fire.WasPressedThisFrame() && character.currentPower >= firePowerCost)
        
        {
            Fire();
            //射击能量消耗
            character.OnFire(firePowerCost);
            playerController.isAttack = true;
            timer = interval;
        }
    }

    private void Fire()
    {
        anim.SetTrigger("Shoot");
        //生成子弹
        //GameObject bullet = Instantiate(bulletPrefab, (Vector2)this.gameObject.transform.position + gunEnd, Quaternion.identity);
        //在对象池中获得子弹
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = this.transform.position;
        //Debug.Log("Gun.this.transform.position:" + this.transform.position);
        bullet.transform.localScale = this.transform.localScale;
        bullet.GetComponent<Bullet>().SetSpeed();
    }
}
