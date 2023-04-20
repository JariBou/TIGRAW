using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BraceletUpgradeConnection : MonoBehaviour
{
    private Image _image;
    private RectTransform _rectTransform;

    public BraceletUpgradeButton prevUpgrade;
    public BraceletUpgradeButton nextUpgrade;


    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public RectTransform GetRectTransform()
    {
        return _rectTransform;
    }
}
