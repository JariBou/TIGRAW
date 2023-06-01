using System;
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

        private void Awake()
        {
            _playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
            _heatManager = GameObject.FindWithTag("Player").GetComponent<HeatManager>();
            baseWidth = container.rect.width; // Height and width are reversed because otherwise shit goes crazy
        }

        void Start()
        {
            // _playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
            // _heatManager = _playerScript.heatManager;
        
            
            OverdriveBarResize();

            HeatLevel = _heatManager.GetHeatLevel();

            FlammesReset();

            // for (int i = 0; i < _heatManager.MaxHeatLevel-1; i++)
            // {
            //     flammes[i].gameObject.SetActive(true);
            // }

            for (int i = 0; i < HeatLevel; i++)
            {
                flammes[i].Toggle();
            }
        }

        void OnBraceletUpgrade(BraceletUpgrades upgrade)
        {
            // Debug.LogWarning("OnBraceletUpgrade");
            if (upgrade == BraceletUpgrades.BonusMaxHeat)
            {
                OverdriveBarResize();
            } else if (upgrade is BraceletUpgrades.NewOverdriveLevel or BraceletUpgrades.NewOverdriveLevels)
            {
                FlammesReset();
            }
        }

        public void OverdriveBarResize()
        {
            float heatRatio = _heatManager.GetMaxHeat() / _heatManager.baseMaxHeat;

            container.sizeDelta = new Vector2(baseWidth * heatRatio, container.sizeDelta.y);
        }

        public void FlammesReset()
        {
            Debug.Log("Resetting Flammes");
            for (int i = 0; i < flammes.Count; i++)
            {
                flammes[i].gameObject.SetActive(false);

                if (i < _heatManager.GetMaxHeatLevel()-1)
                {
                    flammes[i].gameObject.SetActive(true);
                }
            }

        }

        void FixedUpdate()
        {
            if (_heatManager.GetHeatLevel() != HeatLevel)
            {
                int diff = _heatManager.GetHeatLevel() - HeatLevel; // 1 = we light one up, -1 = we turn one off
                if (HeatLevel>flammes.Count || HeatLevel<0){return;}
                if (diff == 1)
                {
                    if (flammes[HeatLevel].isActiveAndEnabled)
                    {
                        flammes[HeatLevel].Toggle();
                    }
                } else if (diff == -1)
                {
                    if (flammes[HeatLevel-1].isActiveAndEnabled)
                    {
                        flammes[HeatLevel-1].Toggle();
                    }
                }
                HeatLevel += diff;
            }
        
        
            int currHeat = _heatManager.GetHeat();
            float ratio = currHeat / _heatManager.GetMaxHeat();

            fillImg.fillAmount = ratio;
        }
    }
}
