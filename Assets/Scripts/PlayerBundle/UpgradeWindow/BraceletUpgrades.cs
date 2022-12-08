using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraceletUpgrades
{
    public enum UpgradeType
    {
        
    }

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
