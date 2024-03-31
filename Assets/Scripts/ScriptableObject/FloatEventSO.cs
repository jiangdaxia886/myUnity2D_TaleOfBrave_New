using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Event/FloatEventSo")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;


    public void RaiseEvent(float amount)
    {
        OnEventRaised?.Invoke(amount);
    }
}
