using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
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
        //������ɢ��С
        this.transform.localScale = this.transform.localScale + Vector3.one * WaveSpeed * Time.deltaTime;
        //����Ť��ǿ��
        this.material.SetFloat("_DistortIntensity", (1 - Mathf.Clamp(liveTime / TotalTime, 0, 1)) * Intensity);
    }

    public void Ripple(Vector3 position, Vector2 localScale, Vector2 offset)
    {
        this.gameObject.SetActive(true);
        this.transform.localScale = this.localScale;
        this.transform.position = new Vector2(position.x, position.y + offset.y);
        this.liveTime = 0;
    }



}
