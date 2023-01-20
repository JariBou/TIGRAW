using System.Collections.Generic;

namespace PlayerBundle.BraceletUpgrade
{
    public enum UpgradeType
    {
        
    }

    public class BraceletUpgrades
    {
  

        private readonly List<UpgradeType> _unlockedUpgradesTypeList;

        public BraceletUpgrades()
        {
            _unlockedUpgradesTypeList = new List<UpgradeType>();
        }

        public void UnlockUpgrade(UpgradeType upgradeType)
        {
            _unlockedUpgradesTypeList.Add(upgradeType);
        }

        public bool IsUpgradeUnlocked(UpgradeType upgradeType)
        {
            return _unlockedUpgradesTypeList.Contains(upgradeType);
        }

    }
}