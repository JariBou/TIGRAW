using System;
using PlayerBundle.BraceletUpgrade;
using Unity.VisualScripting;
using UnityEngine;


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
        firstUpgrade.SetInitialState(isFirst: true);
        StartCoroutine(firstUpgrade.InitConnections());
    }

    public void ResetUpgrades()
    {
        foreach (Transform connection in connectionsParent.transform)
        {
            Destroy(connection.gameObject);
        }
        gameManager.BraceletUpgradesHandler.Reset();
        
        gameManager.uiController.OverdriveDisplayScript.FlammesReset();
        gameManager.uiController.OverdriveDisplayScript.OverdriveBarResize();
        
        firstUpgrade.SetInitialState(isFirst: true);
        StartCoroutine(firstUpgrade.InitConnections());
    }
}
