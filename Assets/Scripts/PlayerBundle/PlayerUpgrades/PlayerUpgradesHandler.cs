using System;
using System.Collections.Generic;

namespace PlayerBundle.PlayerUpgrades
{
    [Serializable]
    public class PlayerUpgradesHandler
    {
        public static Dictionary<PlayerUpgrades, int> playerUpgradesLvl; // Hashtable to be able to serialize, might do a dictionary with a custom serialisation tho
        public static Dictionary<PlayerUpgrades, float> upgradeAmount;
        
        


        public PlayerUpgradesHandler()
        {
            playerUpgradesLvl = new Dictionary<PlayerUpgrades, int>();
            upgradeAmount = new Dictionary<PlayerUpgrades, float>();
            
            
            PlayerUpgrades[] playerUpgradesArray = (PlayerUpgrades[])Enum.GetValues(typeof(PlayerUpgrades));
            foreach (var upgrade in playerUpgradesArray)
            {
                playerUpgradesLvl.Add(upgrade, 0); // Defaults to lvl 0
                upgradeAmount.Add(upgrade, 0.1f); // Defaults to 0.1 upgrading
            }
        }

        public static float GetUpgradedAmount(PlayerUpgrades upgrade)
        {
            return (float)Math.Round(playerUpgradesLvl[upgrade] * upgradeAmount[upgrade], 3);
        }

        public static ref float GetPlayerRef(PlayerUpgrades upgrade)
        {
            switch (upgrade)
            {
                case PlayerUpgrades.AtkMultiplier:
                    return ref Player.Instance.baseAtkMultiplier;
                case PlayerUpgrades.Health:
                    return ref Player.Instance.baseMaxHealth;
            }

            throw new NotImplementedException($"Upgrade '{upgrade}' is not implemented!");
        }


        public static void Upgrade(PlayerUpgrades upgrade)
        {
            switch (upgrade)
            {
                case PlayerUpgrades.AtkMultiplier:
                    Player.Instance.baseAtkMultiplier += upgradeAmount[upgrade];
                    break;
                case PlayerUpgrades.Health:
                    Player.Instance.health += upgradeAmount[upgrade];
                    Player.Instance.baseMaxHealth += upgradeAmount[upgrade];
                    break;
                default:
                    return;
            }
            
            playerUpgradesLvl[upgrade] = (int)playerUpgradesLvl[upgrade] + 1;

        }
        
        // Kind of problematic for bool or upgrades that upgrade
        // multiple stats, maybe return an array of refs and iterate
        // through them since we can get the upgrade amounts for
        // each.... seems hard tho
        public static void Upgrade2(PlayerUpgrades upgrade) 
        {
            GetPlayerRef(upgrade) += upgradeAmount[upgrade];
            
            playerUpgradesLvl[upgrade] = (int)playerUpgradesLvl[upgrade] + 1;
        }
    }
}