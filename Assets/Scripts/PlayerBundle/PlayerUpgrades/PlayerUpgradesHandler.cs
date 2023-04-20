using System;
using System.Collections.Generic;

namespace PlayerBundle.PlayerUpgrades
{
    
    public enum PlayerUpgrades
    {
        AtkMultiplier,
        Health,
    }
    public class PlayerUpgradesHandler
    {
        public Dictionary<PlayerUpgrades, int> UpgradesLvl; // Hashtable to be able to serialize, might do a dictionary with a custom serialisation tho, nvm
        public Dictionary<PlayerUpgrades, float> UpgradesAmount;
        

        public PlayerUpgradesHandler()
        {
            UpgradesLvl = new Dictionary<PlayerUpgrades, int>();
            UpgradesAmount = new Dictionary<PlayerUpgrades, float>();
            
            
            PlayerUpgrades[] playerUpgradesArray = (PlayerUpgrades[])Enum.GetValues(typeof(PlayerUpgrades));
            foreach (var upgrade in playerUpgradesArray)
            {
                UpgradesLvl.Add(upgrade, 0); // Defaults to lvl 0
                UpgradesAmount.Add(upgrade, 0.1f); // Defaults to 0.1 upgrading
            }
        }


        public float GetUpgradedAmount(PlayerUpgrades upgrade)
        {
            return (float)Math.Round(UpgradesLvl[upgrade] * UpgradesAmount[upgrade], 3);
        }

        public static ref float GetPlayerRef(PlayerUpgrades upgrade) // This is useless for now and probably forever
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


        public void Upgrade(PlayerUpgrades upgrade)
        {
            // Switch useless now because we calculate bonuses based on multiplication between upgradeAmount and UpgradeLvl
            // switch (upgrade)
            // {
            //     case PlayerUpgrades.AtkMultiplier:
            //         _player.baseAtkMultiplier += upgradeAmount[upgrade];
            //         break;
            //     case PlayerUpgrades.Health:
            //         _player.health += upgradeAmount[upgrade];
            //         _player.baseMaxHealth += upgradeAmount[upgrade];
            //         break;
            //     default:
            //         return;
            // }
            
            // TODO: Heal player when upgrading health OR do smth where teleporters have an option to apply an effect such as heal player
            // TODO: 2nd One works best imo
            
            UpgradesLvl[upgrade] = (int)UpgradesLvl[upgrade] + 1;

        }
        
        // Kind of problematic for bool or upgrades that upgrade
        // multiple stats, maybe return an array of refs and iterate
        // through them since we can get the upgrade amounts for
        // each.... seems hard tho
        public void Upgrade2(PlayerUpgrades upgrade) 
        {
            GetPlayerRef(upgrade) += UpgradesAmount[upgrade];
            
            UpgradesLvl[upgrade] = (int)UpgradesLvl[upgrade] + 1;
        }
    }
}