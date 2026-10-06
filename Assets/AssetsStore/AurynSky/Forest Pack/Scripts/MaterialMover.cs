using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialMover : MonoBehaviour
{

    public float scrollSpeed = 0.5F;
    public Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
        //Debug.Log(Time.time);
        //Debug.Log(Time.time * scrollSpeed);
    }
    void Update()
    {
        //Debug.Log(Time.time);
        float offset = Time.time * scrollSpeed;
        //Debug.Log(offset);
        //rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        rend.material.mainTextureOffset = new Vector2(0, offset);
    }
}