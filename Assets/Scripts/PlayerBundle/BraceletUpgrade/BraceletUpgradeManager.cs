using System;
using System.Collections;
using System.Collections.Generic;
using PlayerBundle.BraceletUpgrade;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


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
    
    [Header("Badges")]
    public Sprite badgeForbidden;
    public Sprite badgeMaxedOut;
    
    [Header("Utils")]
    public GameObject connectionsParent;
    public GameObject connectionPrefab;

    public BraceletUpgradeButton firstUpgrade;

    public Tooltip Tooltip;

    public GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(firstUpgrade.InitConnections());
    }

  
}
