using System.Collections.Generic;
using MainMenusScripts;
using PlayerBundle;
using PlayerBundle.BraceletUpgrade;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
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

        private void OnEnable()
        {
            BraceletUpgradeButton.OnBraceletUpgrade += OnBraceletUpgrade;
        }

        private void OnDisable()
        {
            BraceletUpgradeButton.OnBraceletUpgrade -= OnBraceletUpgrade;
        }

        void Start()
        {
            _playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
            _heatManager = _playerScript.heatManager;
        
            baseWidth = container.rect.width; // Height and width are reversed because otherwise shit goes crazy
            float heatRatio = _heatManager.MaxHeat / _heatManager.baseMaxHeat;

            container.sizeDelta = new Vector2(baseWidth * heatRatio, container.sizeDelta.y);

            HeatLevel = _heatManager.GetHeatLevel();

            for (int i = 0; i < HeatLevel; i++)
            {
                flammes[i].Toggle();
            }
        }

        void OnBraceletUpgrade(BraceletUpgrades upgrade)
        {
            if (upgrade == BraceletUpgrades.BonusMaxHeat)
            {
                float heatRatio = _heatManager.MaxHeat / _heatManager.baseMaxHeat;

                container.sizeDelta = new Vector2(baseWidth * heatRatio, container.sizeDelta.y);
            }
        }

        void FixedUpdate()
        {
            if (_heatManager.GetHeatLevel() != HeatLevel)
            {
                int diff = _heatManager.GetHeatLevel() - HeatLevel; // 1 = we light one up, -1 = we turn one off
                if (diff == 1)
                {
                    if (flammes[HeatLevel].isActiveAndEnabled)
                    {
                        flammes[HeatLevel].Toggle();
                    }
                } else if (diff == -1)
                {
                    if (flammes[HeatLevel].isActiveAndEnabled)
                    {
                        flammes[HeatLevel-1].Toggle();
                    }
                }
                HeatLevel += diff;
            }
        
        
            int currHeat = _heatManager.GetHeat();
            float ratio = currHeat / _heatManager.MaxHeat;

            fillImg.fillAmount = ratio;
        }
    }
}
