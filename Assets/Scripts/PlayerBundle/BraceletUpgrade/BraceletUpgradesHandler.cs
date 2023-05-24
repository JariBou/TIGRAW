using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBundle.BraceletUpgrade
{
    public enum BraceletUpgrades
    {
        RecoveryRateIncrease,
        BonusRecoveryRateIncrease,
        BonusMaxHeat,
        BonusDmgOnHeatLvl,
        ReduceHeatLvl,
    }

    public class BraceletUpgradesHandler
    {
        private Dictionary<BraceletUpgrades, int> UpgradesLvl; 
        public Dictionary<BraceletUpgrades, float> UpgradesAmount;
        

        public BraceletUpgradesHandler()
        {
            UpgradesLvl = new Dictionary<BraceletUpgrades, int>();
            UpgradesAmount = new Dictionary<BraceletUpgrades, float>();
            
            
            BraceletUpgrades[] playerUpgradesArray = (BraceletUpgrades[])Enum.GetValues(typeof(BraceletUpgrades));
            foreach (var upgrade in playerUpgradesArray)
            {
                // This means some buttons will have to start with lvl 1 to make it easier for some effects maybe
                UpgradesLvl.Add(upgrade, 0); // Defaults to lvl 0
                UpgradesAmount.Add(upgrade, 0.1f); // Defaults to 0.1 upgrading
            }
        }


        public float GetUpgradedAmount(BraceletUpgrades upgrade)
        {
            return (float)Math.Round(UpgradesLvl[upgrade] * UpgradesAmount[upgrade], 3);
        }

        public static ref float GetPlayerRef(BraceletUpgrades upgrade) // This is useless for now and probably forever
        {
            switch (upgrade)
            {
                
            }

            throw new NotImplementedException($"Upgrade '{upgrade}' is not implemented!");
        }


        public void Upgrade(BraceletUpgrades upgrade)
        {

            
            UpgradesLvl[upgrade] = (int)UpgradesLvl[upgrade] + 1;

        }
        
    
    }
}