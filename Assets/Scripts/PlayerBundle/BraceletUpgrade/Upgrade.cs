using System;

namespace PlayerBundle.BraceletUpgrade
{
    [Serializable]
    public class Upgrade
    {

        public string name;
        public int level;
        public int maxLevel;
    
        public UpgradeType upgradeType;

        public bool IsUpgradable()
        {
            return level < maxLevel;
        }

        public int Reset() // Resets upgrade level and return number of times it was upgraded
        {
            int levelsUpgraded = level;
            level = 0;
            return levelsUpgraded;
        }


    }
}
