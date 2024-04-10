using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //�ӵ��ٶ�
    public float speed;

    //��ը��Ч
    public GameObject explosionPrefab;

    //��ȡ����
    private Rigidbody2D rb;
    //��ù�������ͼ��
    public LayerMask attackLayer;

    public float checkDistance;
    //�ӵ��ƶ�����
    private Vector3 moveDir;
    //����Ŀ��λ��
    private Vector3 target;

    private double dis = 99999;

    private Collider2D targetEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetEnemy = null;


    }

    //�ӵ������ٶȷ���
    public void SetSpeed(Vector2 direction) 
    {
        //rb.velocity = direction * speed;
        rb.velocity = (Vector2)FoundTarget() * speed;
    }

    public Vector3 FoundTarget()
    {
        //�ж���transform.positionΪ���ĵ㣬�뾶ΪcheckDistance��Χ���Ƿ���attackLayer
        Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, checkDistance, attackLayer);
        //Debug.Log("obj[0].name:"+obj[0].name);
        double tmp;
        double tan;
        //�泯����
        int direction = this.transform.localScale.x > 0 ? 1 : -1;
        //Debug.Log("this.transform.localScale.x :" + this.transform.localScale.x);
        //���˷���
        int enemyDirection;
        if (obj.Length  > 0)
        {
            //Ѱ���������
            foreach (var i in obj)
            {
                enemyDirection = i.transform.position.x - this.gameObject.transform.position.x > 0 ? 1 : -1;
                //����˾���
                tmp = Math.Pow(i.transform.position.x - this.gameObject.transform.position.x, 2) + Math.Pow(i.transform.position.y - this.gameObject.transform.position.y, 2);
                //��������ҽǶ�
                tan = Math.Abs((i.transform.position.y - this.gameObject.transform.position.y) / (i.transform.position.x - this.gameObject.transform.position.x));
                //����˵���������泯����45�Ƚ����ڣ�����������ĵ��ˣ����л�Ŀ��
                if (tmp < dis && enemyDirection == direction && tan <= 1 )
                {
                    dis = tmp;
                    targetEnemy = i;
                }
            }
            if (targetEnemy != null)
            {
                //���˵����ĵ����ڽŵף���Ϊ�и��ʱ��ѡ����bottom������Ŀ����y����Ҫ���0.5f
                //Ѱ��Ŀ���
                target = new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y + 0.5f, 0);
                //�趨�ƶ�����
                moveDir = (target - this.gameObject.transform.position).normalized;
                return moveDir;
            }
        }
        return new Vector3(this.transform.localScale.x, 0,0);
    }




    //��ײ���������ɱ�ը��Ч�������ӵ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        //���ɱ�ը��Ч
        GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.localScale = rb.velocity.x > 0 ? new Vector3(1,1,1) : new Vector3(-1,1,1);
        //��ͣ�ӵ�Я��
        StartCoroutine(Stop());
        Destroy(this.gameObject);


    }

    //��Ϊ���е��˺�����ܻ�character.cs��TakeDamage()����������attack�����������destroy���ᱨ�Ҳ����������
    IEnumerator Stop()
    {
        rb.velocity = Vector2.zero;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        
    }
}
