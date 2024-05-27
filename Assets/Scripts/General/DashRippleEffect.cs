using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRippleEffect : MonoBehaviour
{
    public int WaveSpeed;
    public float TotalTime;
    public Material material;
    //起始缩放比率
    private Vector2 scale1;
    //结束缩放比率
    private Vector2 scale2;

    private float liveTime;
    private void Awake()
    {
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
        //波纹扩散大小
        this.transform.localScale = this.transform.localScale + Vector3.one * WaveSpeed * Time.deltaTime;
    }

    public void DashRipple(Vector3 position)
    {
        this.gameObject.SetActive(true);
        this.transform.localScale = this.scale1;
        this.transform.position = position;
        this.liveTime = 0;
    }
}
