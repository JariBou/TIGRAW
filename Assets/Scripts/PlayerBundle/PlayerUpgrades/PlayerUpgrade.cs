using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace PlayerBundle.PlayerUpgrades
{
    [Serializable]
    public enum PlayerUpgrades
    {
        AtkMultiplier,
        Health,
    }

    public class PlayerUpgrade : MonoBehaviour
    {
        public TMP_Text value;
        public TMP_Text upgradeAmountText;
        public PlayerUpgrades upgrade;
        public float upgradeAmount;

        private Player player;
        private GameManager _gm;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }


        // Start is called before the first frame update
        void Start()
        {
            
            if (_gm.PlayerUpgradesHandler.UpgradesAmount.ContainsKey(upgrade))
            {
                upgradeAmount = _gm.PlayerUpgradesHandler.UpgradesAmount[upgrade];
            }
            else
            {
                _gm.PlayerUpgradesHandler.UpgradesAmount.Add(upgrade, upgradeAmount);
            }

            Refresh();
        }

        public void Refresh()
        {
            switch (upgrade)
            {
                case PlayerUpgrades.AtkMultiplier:
                    Refresh(Math.Round(player.AtkMultiplier, 3).ToString(CultureInfo.InvariantCulture));
                    break;
                case PlayerUpgrades.Health:
                    Refresh(player.MaxHealth.ToString(CultureInfo.InvariantCulture));
                    break;
            }
        }

        private void Refresh(string text)
        {
            value.text = text;
            upgradeAmountText.text = $"{(upgradeAmount > 0 ? '+' : '-')} {upgradeAmount}";
        }
    
        public void Upgrade()
        {
            _gm.PlayerUpgradesHandler.Upgrade(upgrade);
            Refresh();
        }
    }
}