using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRippleEffect : MonoBehaviour
{
    //ǿ��
    public int Intensity;
    public int WaveSpeed;
    public float TotalTime;
    public Material material;
    //��ʼ���ű���
    private Vector2 scale1;
    //�������ű���
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
        //������ɢ��С
        //�����������д��
        //this.transform.localScale = new Vector3 (this.transform.localScale.x +  WaveSpeed * Time.deltaTime * this.transform.localScale.x, this.transform.localScale.y + WaveSpeed * Time.deltaTime * this.transform.localScale.y, this.transform.localScale.z);
        //�õ�normal from height�ڵ�,���ڱ༭�����ڵ����ˣ�����Ҫÿ֡���±仯transform.scale������ʾ�����棬ԭ���̽��(��my distortion����Heat Haze Overlay����Ҳ����������my distortion�� distortion���������� distortion����normal from height�ڵ�)
        this.transform.localScale = this.transform.localScale + new Vector3(this.transform.localScale.x, this.transform.localScale.y, Vector3.one.z) * WaveSpeed * Time.deltaTime;
        //������ɢ����
        if (this.localScale.x > 0)
        {
            this.material.SetVector("_RippleVector", new Vector2(1, 0.5f));
        }
        else
        {
            this.material.SetVector("_RippleVector", new Vector2(0, 0.5f));
        }
        //����Ť��ǿ��
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
