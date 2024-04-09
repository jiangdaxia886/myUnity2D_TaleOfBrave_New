using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    //�ӵ�ʱ����
    public float interval;
    //�ӵ�
    public GameObject bulletPrefab;
    //�����ӵ���ǹ��λ��
    public Vector2 gunEnd;
    //�ӵ��������
    public Vector2 direction;
    //��ʱ��
    public float timer = 0;

    private Animator anim;

    private PlayerController playerController;

    //��unity�Զ����ɵ�PlayerInputController��������
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
        //���������
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
