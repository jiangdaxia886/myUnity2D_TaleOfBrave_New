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
        //������ʼλ��
        transform.position = new Vector3(cameraTransform.position.x + BackGroundOffset.x, cameraTransform.position.y + BackGroundOffset.y, 0f);
        //��ȡͼƬ����
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        //����ͼƬ���
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
        Debug.Log("sprite.pixelsPerUnit:"+sprite.pixelsPerUnit+ ";texture.width:"+ texture.width + ";textureUnitSizeX:"+ textureUnitSizeX);
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        //�ƶ�����
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        //��ǰ����ͼƬ�ƶ����ƶ����ȳ���˥��ϵ��parallaxEffectMultiplier
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;
        //�����ѡ��ˮƽ����ˢ��
        if (infiniteHorizontal)
        {
            //Debug.Log("cameraTransform.position.x - transform.position.x:" + (cameraTransform.position.x - transform.position.x)+ "cameraTransform.position.x:"+ cameraTransform.position.x+ "transform.position.x:"+ transform.position.x);
            //��Ϊ�����ƶ�����˥���ģ��ý����ݵر�����˵��������ƶ����ݵز��ƶ�����ʱ��������λ��-�ݵ�λ��>�ݵصĳ��ȣ������¶�λ��ǰ����
            //textureUnitSizeX �Ǳ���ͼƬ�ĳ��ȣ�����������ƶ���������ͼƬ��һ���Ӧ���ƶ������Դ˴����ַ������ٽ�ͼƬ��sprite renderer��width����Ϊ��ͼƬԭʼ���ȵ�2�����ڽ��˴�textureUnitSizeX /2
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
