using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    public Color color ;

    [Header("时间控制参数")]
    public float activeTime;//显示时间
    public float activeStart;//开始显示的时间点

    [Header("不透明度控制")]
    private float alpha;
    public float alphaSet;//初始值
    public float alphaMultiplier;//渐变

    private void OnEnable()
    {
        color = new Color(0.8f, 0.7f, 0.5f,1f);
        //找到玩家物体
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        //显示时间点
        activeStart = Time.time;
    }

    void Update()
    {
        //残影逐渐透明
        alpha *= alphaMultiplier;

        color.a = alpha;//Color(1,1,1,1)代表100%显示各通道颜色，请查看Api手册

        thisSprite.color = color;
        //Debug.Log("thisSprite.color"+thisSprite.color);

        if (Time.time >= activeStart + activeTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
