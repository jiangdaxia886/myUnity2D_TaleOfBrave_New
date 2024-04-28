using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem vfxJumpDust;

    [SerializeField]
    private ParticleSystem vfxLandDust;

    //»¬Ç½»Ò³¾
    [SerializeField]
    private ParticleSystem vfxWallSlide;

    [SerializeField]
    private Color color;



    public void JumpDust(Vector3 position, Vector2 dir)
    {
        Debug.Log("playJumpDust!!!");
        this.vfxJumpDust.transform.position = position;
        this.vfxJumpDust.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
        var main = this.vfxJumpDust.main;
        main.startColor = this.color;
        this.vfxJumpDust.Play();
    }

    public void LandDust(Vector3 position)
    {
        Debug.Log("playLandDust!!!:"+ position);
        this.vfxLandDust.transform.position = position;
        var main = this.vfxLandDust.main;
        main.startColor = this.color;
        this.vfxLandDust.Play();
    }

    public void WallSlide(Vector3 position,Vector2 dir)
    {
        this.vfxWallSlide.transform.position = position;
        this.vfxWallSlide.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
        var main = this.vfxWallSlide.main;
        main.startColor = this.color; 
        this.vfxWallSlide.Emit(1);
    }
}
