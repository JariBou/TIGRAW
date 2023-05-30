using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace PlayerBundle.PlayerUpgrades
{
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
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }


        // Start is called before the first frame update
        void Start()
        {
            if (_gm.PlayerUpgradesHandler.UpgradesAmount.TryGetValue(upgrade, out var value))
            {
                upgradeAmount = value;
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