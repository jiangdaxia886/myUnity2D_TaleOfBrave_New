using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    public Color color ;

    [Header("ʱ����Ʋ���")]
    public float activeTime;//��ʾʱ��
    public float activeStart;//��ʼ��ʾ��ʱ���

    [Header("��͸���ȿ���")]
    private float alpha;
    public float alphaSet;//��ʼֵ
    public float alphaMultiplier;//����

    private void OnEnable()
    {
        color = new Color(0.8f, 0.7f, 0.5f,1f);
        //�ҵ��������
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        //��ʾʱ���
        activeStart = Time.time;
    }

    void Update()
    {
        //��Ӱ��͸��
        alpha *= alphaMultiplier;

        color.a = alpha;//Color(1,1,1,1)����100%��ʾ��ͨ����ɫ����鿴Api�ֲ�

        thisSprite.color = color;
        //Debug.Log("thisSprite.color"+thisSprite.color);

        if (Time.time >= activeStart + activeTime)
        {
            //���ض����
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
