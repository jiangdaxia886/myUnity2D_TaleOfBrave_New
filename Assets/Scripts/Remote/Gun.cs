using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Gun : MonoBehaviour
{
    //�ӵ�ʱ����
    public float interval;
    //�ӵ�
    public GameObject bulletPrefab;
    //�ӵ��������
    public Vector2 direction;
    //��ʱ��
    public float timer = 0;

    public int firePowerCost;

    private Animator anim;

    private PlayerController playerController;

    private Character character;

    public GameObject player;

    //��unity�Զ����ɵ�PlayerInputController��������
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
        //����������������������
        else if(inputController.GamePlay.Fire.WasPressedThisFrame() && character.currentPower >= firePowerCost)
        
        {
            Fire();
            //�����������
            character.OnFire(firePowerCost);
            playerController.isAttack = true;
            timer = interval;
        }
    }

    private void Fire()
    {
        anim.SetTrigger("Shoot");
        //�����ӵ�
        //GameObject bullet = Instantiate(bulletPrefab, (Vector2)this.gameObject.transform.position + gunEnd, Quaternion.identity);
        //�ڶ�����л���ӵ�
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = this.transform.position;
        //Debug.Log("Gun.this.transform.position:" + this.transform.position);
        bullet.transform.localScale = this.transform.localScale;
        bullet.GetComponent<Bullet>().SetSpeed();
    }
}
