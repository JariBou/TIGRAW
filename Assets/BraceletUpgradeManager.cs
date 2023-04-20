using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum BraceletUpgrades
{
    
}

public class BraceletUpgradeManager : MonoBehaviour
{
    [Header("Connections")] 
    public Sprite connectionHidden;
    public Sprite connectionLocked;
    public Sprite connectionUnlocked;
    
    [Header("Upgrades")]
    public Sprite upgradeHidden;
    public Sprite upgradeLocked;
    public Sprite upgradeUnlocked;
    
    [Header("Utils")]
    public GameObject connectionsParent;
    public GameObject connectionPrefab;

    public BraceletUpgradeButton firstUpgrade;


    private void Start()
    {
        StartCoroutine(firstUpgrade.InitConnections());
    }
}
