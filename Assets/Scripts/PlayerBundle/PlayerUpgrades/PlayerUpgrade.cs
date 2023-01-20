using System.Globalization;
using TMPro;
using UnityEngine;

namespace PlayerBundle.PlayerUpgrades
{
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
    
        //TODO: Refresh on Load bitch
    
        // Start is called before the first frame update
        void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            switch (upgrade)
            {
                case PlayerUpgrades.AtkMultiplier:
                    Refresh(Player.instance.atkMultiplier.ToString(CultureInfo.InvariantCulture));
                    break;
                case PlayerUpgrades.Health:
                    Refresh(Player.instance.health.ToString(CultureInfo.InvariantCulture));
                    break;
            }
        }

        public void Refresh(string text)
        {
            value.text = text;
            upgradeAmountText.text = $"{(upgradeAmount > 0 ? '+' : '-')} {upgradeAmount}";
        }
    
        public void Upgrade()
        {
            switch (upgrade)
            {
                case PlayerUpgrades.AtkMultiplier:
                    Player.instance.atkMultiplier += upgradeAmount;
                    Refresh(Player.instance.atkMultiplier.ToString(CultureInfo.InvariantCulture));
                    break;
                case PlayerUpgrades.Health:
                    Player.instance.health += upgradeAmount;
                    Refresh(Player.instance.health.ToString(CultureInfo.InvariantCulture));
                    break;
            }
        }
    }
}