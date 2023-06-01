using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UpgradesZoomScript : MonoBehaviour
{
    public RectTransform panel;
    private Vector2 zoom = Vector2.one;

    public void Zoom()
    {
        zoom += Vector2.one * 0.1f;
        panel.localScale = Vector3.one * zoom;
    }

    public void UnZoom()
    {
        zoom += Vector2.one * -0.1f;
        panel.localScale = Vector3.one * zoom;
    }

}
