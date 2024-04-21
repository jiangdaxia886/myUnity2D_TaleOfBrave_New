using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteHorizontal;
    [SerializeField] private bool infiniteVertical;

    public Vector2 BackGroundOffset;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        //背景初始位置
        transform.position = new Vector3(cameraTransform.position.x + BackGroundOffset.x, cameraTransform.position.y + BackGroundOffset.y, 0f);
        //获取图片纹理
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        //背景图片宽度
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
        Debug.Log("sprite.pixelsPerUnit:"+sprite.pixelsPerUnit+ ";texture.width:"+ texture.width + ";textureUnitSizeX:"+ textureUnitSizeX);
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        //移动长度
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        //当前背景图片移动，移动长度乘以衰减系数parallaxEffectMultiplier
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;
        //如果勾选了水平背景刷新
        if (infiniteHorizontal)
        {
            //Debug.Log("cameraTransform.position.x - transform.position.x:" + (cameraTransform.position.x - transform.position.x)+ "cameraTransform.position.x:"+ cameraTransform.position.x+ "transform.position.x:"+ transform.position.x);
            //因为背景移动是有衰减的，拿近处草地背景来说，摄像机移动，草地不移动，此时如果摄像机位置-草地位置>草地的长度，则重新定位当前背景
            //textureUnitSizeX 是背景图片的长度，但是摄像机移动超过背景图片的一半就应该移动，所以此处两种方案，①将图片的sprite renderer的width调整为超图片原始长度的2倍；②将此处textureUnitSizeX /2
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (infiniteVertical)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
            }
        }
    }
}
