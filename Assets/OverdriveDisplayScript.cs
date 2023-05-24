using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MainMenusScripts;
using PlayerBundle;
using UnityEngine;
using UnityEngine.UI;

public class OverdriveDisplayScript : MonoBehaviour
{
    [SerializeField]
    private Image fillImg;

    [SerializeField] private RectTransform container;
    private float baseWidth;

    [SerializeField] private List<UIAnimation> flammes; // there should only be 2 flammes, or maybe have one since the beginning
    private int HeatLevel = 0;

    private Player _playerScript;
    private HeatManager _heatManager;
    void Start()
    {
        _playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        _heatManager = _playerScript.heatManager;
        
        baseWidth = container.rect.width; // Height and width are reversed because otherwise shit goes crazy
        float heatRatio = _heatManager.MaxHeat / _heatManager.baseMaxHeat;

        container.sizeDelta = new Vector2(baseWidth * heatRatio, container.sizeDelta.y);

        HeatLevel = _heatManager.GetHeatLevel();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            float heatRatio = _heatManager.MaxHeat / _heatManager.baseMaxHeat;

            container.sizeDelta = new Vector2(baseWidth * heatRatio, container.sizeDelta.y);

        }

        if (_heatManager.GetHeatLevel() != HeatLevel)
        {
            int diff = _heatManager.GetHeatLevel() - HeatLevel; // 1 = we light one up, -1 = we turn one off
            if (diff == 1)
            {
                flammes[HeatLevel].Toggle();
            } else if (diff == -1)
            {
                flammes[HeatLevel-1].Toggle();
            }
            HeatLevel += diff;
        }
        
        
        int currHeat = _heatManager.GetHeat();
        float ratio = currHeat / _heatManager.MaxHeat;

        fillImg.fillAmount = ratio;
    }
}
