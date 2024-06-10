using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class ShapeTransform : MonoBehaviour
{
    public float transformDuration;

    public float transformTimer;
    //变换系数
    public float transformFactor;

    private Vector2 awakeScale;

    private int zf;
    private void Awake()
    {
        awakeScale = transform.localScale;

    }

    private void Update()
    {
        if (transformTimer > 0)
        {
            transformTimer -= Time.deltaTime;
            zf = this.transform.localScale.x > 0 ? 1 : -1;
            //形状变换
            this.transform.localScale = new Vector2(Mathf.Lerp(zf * Mathf.Abs(awakeScale.x), zf * Mathf.Abs(awakeScale.x) * (2 - transformFactor) * 1.2f , 1 - Mathf.Abs((transformTimer - transformDuration / 2) / (transformDuration / 2)))
                , Mathf.Lerp(awakeScale.y, awakeScale.y * transformFactor, 1 - Mathf.Abs((transformTimer - transformDuration / 2)/ (transformDuration / 2))));
        }
        else
        {
            transformTimer = 0;
            this.transform.localScale = new Vector2(this.transform.localScale.x, awakeScale.y);
        }
    }
}
