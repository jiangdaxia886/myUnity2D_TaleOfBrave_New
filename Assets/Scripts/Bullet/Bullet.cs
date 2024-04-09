using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //�ӵ��ٶ�
    public float speed;

    //��ը��Ч
    public GameObject explosionPrefab;

    //��ȡ����
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


    }

    //�ӵ������ٶȷ���
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


    //��ײ���������ɱ�ը��Ч�������ӵ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.localScale = rb.velocity.x > 0 ? new Vector3(1,1,1) : new Vector3(-1,1,1);
        Destroy(this.gameObject);
    }
}
