using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/VoidEventSo")]
public class VoidEventSo : ScriptableObject
{
    public UnityAction OnEventRaised;

    /// <summary>
    /// 相机抖动SO方法
    /// </summary>
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
