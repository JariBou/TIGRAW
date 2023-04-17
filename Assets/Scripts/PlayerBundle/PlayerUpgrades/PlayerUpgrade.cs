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
        
    
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (PlayerUpgradesHandler.upgradeAmount.ContainsKey(upgrade))
            {
                PlayerUpgradesHandler.upgradeAmount[upgrade] = upgradeAmount;
            }
            else
            {
                PlayerUpgradesHandler.upgradeAmount.Add(upgrade, upgradeAmount);
            }

            Refresh();
        }

        public void Refresh()
        {
            switch (upgrade)
            {
                case PlayerUpgrades.AtkMultiplier:
                    Refresh(Math.Round(player.baseAtkMultiplier, 3).ToString(CultureInfo.InvariantCulture));
                    break;
                case PlayerUpgrades.Health:
                    Refresh(player.baseMaxHealth.ToString(CultureInfo.InvariantCulture));
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
            PlayerUpgradesHandler.Upgrade(upgrade);
            Refresh();
        }
    }
}