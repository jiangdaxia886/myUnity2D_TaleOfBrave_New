using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRippleEffect : MonoBehaviour
{
    //强度
    public int Intensity;
    public int WaveSpeed;
    public float TotalTime;
    public Material material;
    //起始缩放比率
    private Vector2 scale1;
    //结束缩放比率
    private Vector2 scale2;

    private float liveTime;

    private Vector3 localScale;
    private void Awake()
    {
        this.localScale = Vector3.one;
        this.scale1 = Vector2.one;
        this.scale2 = Vector2.one * 2;
        material = this.GetComponent<SpriteRenderer>().material;
    }


    public void Update()
    {
        if (liveTime >= TotalTime)
        {
           this.gameObject.SetActive(false);
        }
        liveTime += Time.deltaTime;
        Debug.Log("this.transform.localScale.x" + this.transform.localScale.x);
        //波纹扩散大小
        //不能用下面的写法
        //this.transform.localScale = new Vector3 (this.transform.localScale.x +  WaveSpeed * Time.deltaTime * this.transform.localScale.x, this.transform.localScale.y + WaveSpeed * Time.deltaTime * this.transform.localScale.y, this.transform.localScale.z);
        //用到normal from height节点,且在编辑器窗口调用了，必须要每帧更新变化transform.scale才能显示出画面，原理待探究(用my distortion或者Heat Haze Overlay材质也不会这样，my distortion和 distortion的区别在于 distortion用了normal from height节点)
        this.transform.localScale = this.transform.localScale + new Vector3(this.transform.localScale.x, this.transform.localScale.y, Vector3.one.z) * WaveSpeed * Time.deltaTime;
        //波纹扩散方向
        if (this.localScale.x > 0)
        {
            this.material.SetVector("_RippleVector", new Vector2(1, 0.5f));
        }
        else
        {
            this.material.SetVector("_RippleVector", new Vector2(0, 0.5f));
        }
        //波纹扭曲强度
        //this.material.SetFloat("_DistortIntensity", (1 - Mathf.Clamp(liveTime / TotalTime, 0, 1)) * Intensity);
    }

    public void DashRipple(Vector3 position, Vector2 localScale)
    {
        this.gameObject.SetActive(true);
        this.localScale = localScale.x > 0 ? this.scale1 : new Vector2(-this.scale1.x, this.scale1.y);
        this.transform.localScale = this.localScale;
        //Debug.Log("this.transform.localScale" + this.transform.localScale);
        this.transform.position = position;
        this.liveTime = 0;
    }
}
