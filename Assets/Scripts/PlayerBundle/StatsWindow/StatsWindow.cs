using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsWindow : MonoBehaviour
{
    private Canvas canvas;

    public static StatsWindow instance { get; private set; }
    

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        instance = this;
    }

    public void SetCanvasState(bool state)
    {
        canvas.enabled = state;
    }
    
}
