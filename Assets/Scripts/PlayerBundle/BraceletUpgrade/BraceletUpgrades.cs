using System.Collections.Generic;

namespace PlayerBundle.BraceletUpgrade
{
    public enum UpgradeType
    {
        
    }

    public class BraceletUpgrades
    {
  

        private List<UpgradeType> unlockedUpgradesTypeList;

        public BraceletUpgrades()
        {
            unlockedUpgradesTypeList = new List<UpgradeType>();
        }

        public void UnlockUpgrade(UpgradeType upgradeType)
        {
            unlockedUpgradesTypeList.Add(upgradeType);
        }

        public bool IsUpgradeUnlocked(UpgradeType upgradeType)
        {
            return unlockedUpgradesTypeList.Contains(upgradeType);
        }

    }
}