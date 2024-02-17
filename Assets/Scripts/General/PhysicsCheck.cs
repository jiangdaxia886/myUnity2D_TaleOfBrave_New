using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider;

    [Header("������")]
    public bool manual;
    //��ײ��ƫ����
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    //��ײ��Χ
    public float checkRaduis;
    //��ײͼ��
    public LayerMask groundLayer;

    [Header("״̬")]
    public Boolean isGround;

    public bool touchLeftWall;

    public bool touchRightWall;


    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //�������ѡ�ֶ�����
        if (!manual)
        {
            //�Ҳ���ײ���xƫ����������ײ��ķ�Χ/2+��ײ���ƫ������y��λ����y�ı߽�/2
            rightOffset = new Vector2((capsuleCollider.bounds.size.x/2 + capsuleCollider.offset.x), (capsuleCollider.bounds.size.y / 2 ));
            //����x��ƫ���������Ҳ�ƫ�����ĸ�ֵ
            leftOffset = new Vector2(rightOffset.x - capsuleCollider.bounds.size.x, rightOffset.y);
        }
        Check();
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    private void Check()
    {
        //������,����λ��transform.position + bottomOffset��checkRaduis��Χ���Ƿ����groundLayerͼ��,�˴�����new Vector2(bottomOffset.x * transform.localScale.x,bottomOffset.y)����ʾ��ǰ����ǰ������λ����Ϊ���㣬��Ұ��ǰ��������ʱ����ǰ��
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x,bottomOffset.y), checkRaduis, groundLayer);;

        //ǽ���ж�
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);

        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);

    }

    private void OnDrawGizmosSelected()
    {
        //�ڳ����л��Ƽ�ⷶΧ����ײ��Χ���ӻ���
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }

}
