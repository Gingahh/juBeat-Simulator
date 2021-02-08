using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public SpriteRenderer background;
    public float transparency;
    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<SpriteRenderer>();
        //background.color = new Color(1.0f,1.0f,1.0f,transparency);
    }
}
