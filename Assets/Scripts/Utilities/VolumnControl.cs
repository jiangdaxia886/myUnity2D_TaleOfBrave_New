using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumnControl : MonoBehaviour
{
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    public float postExposure;
    // Start is called before the first frame update
    void Start()
    {
/*        List<VolumeComponent> list = volume.profile.components;
        foreach (VolumeComponent component in list)
        {
            Debug.Log(component.ToString());
        }*/
        //获取管线
        volume = GetComponent<Volume>();
        //获得colorAdjustments组件
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        //设置曝光值
        colorAdjustments.postExposure.SetValue(new FloatParameter(postExposure));
    }


}
