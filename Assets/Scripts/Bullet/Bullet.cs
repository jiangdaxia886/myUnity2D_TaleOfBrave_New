using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //子弹速度
    public float speed;

    //爆炸特效
    public GameObject explosionPrefab;

    //获取刚体
    private Rigidbody2D rb;
    //获得攻击对象图层
    public LayerMask attackLayer;

    public float checkDistance;
    //子弹移动方向
    private Vector3 moveDir;
    //敌人目标位置
    private Vector3 target;

    private double dis;

    private Collider2D targetEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        targetEnemy = null;
        dis = 99999;
    }


    //子弹飞行速度方向
    public void SetSpeed() 
    {
        //rb.velocity = direction * speed;
        rb.velocity = (Vector2)FoundTarget() * speed;
    }

    private void Update()
    {
        //SetSpeed();
    }

    public Vector3 FoundTarget()
    {
        //判断以transform.position为中心点，半径为checkDistance范围内是否有attackLayer
        Collider2D[] obj = Physics2D.OverlapCircleAll(transform.position, checkDistance, attackLayer);
        //Debug.Log("obj[0].name:"+obj[0].name);
        double tmp;
        double tan;
        //面朝方向
        int direction = this.transform.localScale.x > 0 ? 1 : -1;
        Debug.Log("this.transform.localScale.x :" + this.transform.localScale.x);
        //敌人方向
        int enemyDirection;
        Debug.Log("Bullet.this.transform.position:" + this.transform.position);
        if (obj.Length  > 0)
        {
            //寻找最近敌人
            foreach (var i in obj)
            {
                enemyDirection = i.transform.position.x - this.gameObject.transform.position.x > 0 ? 1 : -1;
                //与敌人距离
                tmp = Math.Sqrt( Math.Pow(i.transform.position.x - this.gameObject.transform.position.x, 2) + Math.Pow(i.transform.position.y - this.gameObject.transform.position.y, 2));
                //敌人与玩家角度
                tan = Math.Abs((i.transform.position.y - this.gameObject.transform.position.y) / (i.transform.position.x - this.gameObject.transform.position.x));

                //Debug.Log("i.name:" + i.name+ ";tmpdistance:"+tmp+ ";x" + i.transform.position.x+";y:"+ i.transform.position.y+";tan:"+tan);
                //如果此敌人在玩家面朝方向45度角以内，并且是最近的敌人，则切换目标
                if (tmp < dis && enemyDirection == direction && tan <= 1 )
                {
                    dis = tmp;
                    targetEnemy = i;
                }
            }
            if (targetEnemy != null)
            {
                Debug.Log("FoundTarget()!!:"+ targetEnemy.name);
                //敌人的中心点是在脚底，因为切割的时候选的是bottom，所以目标点的y轴需要添加0.5f
                //寻找目标点
                target = new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y + 0.5f, 0);
                //设定移动方向
                moveDir = (target - this.gameObject.transform.position).normalized;
                return moveDir;
            }
        }
        return new Vector3(this.transform.localScale.x, 0,0);
    }




    //碰撞到敌人生成爆炸特效并销毁子弹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        //生成爆炸特效
        //GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        //使用对象池生成爆炸特效
        GameObject explosion = ObjectPool.Instance.GetObject(explosionPrefab);
        explosion.transform.position = this.transform.position;
        explosion.transform.localScale = rb.velocity.x > 0 ? new Vector3(1,1,1) : new Vector3(-1,1,1);

        //暂停子弹携程
        //StartCoroutine(Stop());
        //Destroy(this.gameObject);
        //将该预制体放回对象池
        ObjectPool.Instance.PushObject(this.gameObject);


    }

    //因为命中敌人后敌人受击character.cs中TakeDamage()方法会引用attack对象，如果立刻destroy，会报找不到对象错误
    IEnumerator Stop()
    {
        rb.velocity = Vector2.zero;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        
    }
}
