using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedVignetteScript : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }

    
    void FixedUpdate()
    {
        Color color = image.color;

        if (color.a > 0)
        {
            color.a -= Time.fixedDeltaTime;
            image.color = color;   
        }
    }

    public void ApplyVignette(float alphaValue = 0.8f)
    {
        Color color = image.color;
        color.a = alphaValue;
        image.color = color;
    }
}
