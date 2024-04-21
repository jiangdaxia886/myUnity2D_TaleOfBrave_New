using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ����ǰ����ش��������Ķ�������Żض����
/// </summary>
public class AfterPoolAnimator : MonoBehaviour
{
    private Animator anim;

    private AnimatorStateInfo animatorStateInfo;


    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //��ȡ��ǰ��������
        animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        //���������������ٱ�ը��Ч
        if (animatorStateInfo.normalizedTime >= 1)
        {

            //Destroy(this.gameObject);
            ObjectPool.Instance.PushObject(this.gameObject);
        }
    }
}
