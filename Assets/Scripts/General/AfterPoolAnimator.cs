using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 将当前对象池创建出来的动画物体放回对象池
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
        //获取当前动画进度
        animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        //当动画播放完销毁爆炸特效
        if (animatorStateInfo.normalizedTime >= 1)
        {

            //Destroy(this.gameObject);
            ObjectPool.Instance.PushObject(this.gameObject);
        }
    }
}
