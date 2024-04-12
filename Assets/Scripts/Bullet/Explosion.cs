using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//爆炸特效
public class Explosion : MonoBehaviour
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
