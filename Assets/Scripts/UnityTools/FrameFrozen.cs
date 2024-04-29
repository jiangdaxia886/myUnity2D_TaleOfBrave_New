using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameFrozen : MonoBehaviour
{
    private float ffTimer, ffTimerTotal;

    private float freezeTime;


    private void FixedUpdate()
    {
        if (ffTimer > 0)
        {
            ffTimer -= Time.deltaTime;
            Time.timeScale = Mathf.Lerp(0f, 1f, 1 - (ffTimer / ffTimerTotal));
        }
    }
    public bool UpdateTime(float deltaTime)
    {
        if (freezeTime > 0f)
        {
            freezeTime = Mathf.Max(freezeTime - deltaTime, 0f);
            return false;
        }
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        return true;
    }

    public void frazee(float freezeTime)
    {
        this.ffTimer = freezeTime;
        this.ffTimerTotal = freezeTime;
    }
}
