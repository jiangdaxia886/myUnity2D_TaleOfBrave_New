using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLine : MonoBehaviour
{
    public Texture2D tex;
    public Texture2D _secondaryTex;
    public Material material;
    // Start is called before the first frame update

    private void Awake()
    {
        material = this.GetComponent<SpriteRenderer>().material;
        material.SetTexture("_secondary", _secondaryTex);
    }

}
