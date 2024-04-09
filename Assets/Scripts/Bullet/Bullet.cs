using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //子弹速度
    public float speed;

    //爆炸特效
    public GameObject explosionPrefab;

    //获取刚体
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


    }

    //子弹飞行速度方向
    public void SetSpeed(Vector2 direction) 
    {
        rb.velocity = direction * speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    //碰撞到敌人生成爆炸特效并销毁子弹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.localScale = rb.velocity.x > 0 ? new Vector3(1,1,1) : new Vector3(-1,1,1);
        Destroy(this.gameObject);
    }
}
