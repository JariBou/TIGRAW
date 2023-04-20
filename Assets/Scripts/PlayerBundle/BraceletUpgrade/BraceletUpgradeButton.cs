using System;
using System.Collections;
using System.Collections.Generic;
using PlayerBundle.BraceletUpgrade;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BraceletUpgradeState
{
    Hidden,
    Locked,
    Unlocked
}

public enum BadgeState
{
    Neutral,
    Forbidden,
    MaxedOut
}

public class BraceletUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BraceletUpgrades upgrade;
    public string UpgradeName;
    public string description;
    public int cost;

    public int currentLevel;
    public int maxLevel;
    public float upgradeAmount;

    private Image _buttonImage;
    private RectTransform _rectTransform;
    public Image badgeImage;

    public BraceletUpgradeState state = BraceletUpgradeState.Hidden;
    public BadgeState badgeState = BadgeState.Neutral;

    public BraceletUpgradeManager manager;

    public BraceletUpgradeButton[] nextUpgrades;
    public List<BraceletUpgradeConnection> connections = new(8);
    
    public List<BraceletUpgrades> IncompatibleUpgrades;
    
    public static event Action<BraceletUpgrades> OnBraceletUpgrade;



    private void Awake()
    {
        _buttonImage = GetComponent<Button>().image;
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        OnBraceletUpgrade += OnOtherBraceletUpgrade;
        
        if (manager.gameManager.BraceletUpgradesHandler.UpgradesAmount.ContainsKey(upgrade))
        {
            manager.gameManager.BraceletUpgradesHandler.UpgradesAmount[upgrade] = upgradeAmount;
        }
        else
        {
            manager.gameManager.BraceletUpgradesHandler.UpgradesAmount.Add(upgrade, upgradeAmount);
        }

    }

    private void OnDestroy()
    {
        OnBraceletUpgrade -= OnOtherBraceletUpgrade;
    }

    private void OnOtherBraceletUpgrade(BraceletUpgrades upgrade)
    {
        if (IncompatibleUpgrades.Contains(upgrade))
        {
            badgeState = BadgeState.Forbidden;
            UpdateBadge();
        }
    }

    public RectTransform GetRectTransform()
    {
        return _rectTransform;
    }
    
    public void SetSprite(Sprite sprite)
    {
        _buttonImage.sprite = sprite;
    }

    private void UpdateSprite()
    {
        switch (state)
        {
            case BraceletUpgradeState.Hidden:
                SetSprite(manager.upgradeHidden);
                break;
            case BraceletUpgradeState.Locked:
                SetSprite(manager.upgradeLocked);
                break;
            case BraceletUpgradeState.Unlocked:
                SetSprite(manager.upgradeUnlocked);
                break;
        }
    }
    
    private void UpdateBadge()
    {
        badgeImage.enabled = true;
        switch (badgeState)
        {
            case BadgeState.Neutral:
                badgeImage.enabled = false;
                break;
            case BadgeState.Forbidden:
                badgeImage.sprite = manager.badgeForbidden;
                break;
            case BadgeState.MaxedOut:
                badgeImage.sprite = manager.badgeMaxedOut;
                break;
        }
    }

    public IEnumerator InitConnections()
    {
        UpdateSprite();
        if (nextUpgrades == null) {yield break;}

        foreach (BraceletUpgradeButton upgradeButton in nextUpgrades)
        {
            RectTransform targetRect = upgradeButton.GetRectTransform();

            Vector3 direction =  _rectTransform.position - targetRect.position;
            float distance = direction.magnitude;
            direction /= distance;
            
            float angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI/2) * Mathf.Rad2Deg;
            
            BraceletUpgradeConnection connection = Instantiate(manager.connectionPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward), manager.connectionsParent.transform).GetComponent<BraceletUpgradeConnection>();
            connection.GetRectTransform().sizeDelta = new Vector2(connection.GetRectTransform().sizeDelta.x, distance);

            connection.nextUpgrade = upgradeButton;
            connection.prevUpgrade = this;

            switch (upgradeButton.state)
            {
                case BraceletUpgradeState.Hidden:
                    connection.SetSprite(manager.connectionHidden);
                    break;
                case BraceletUpgradeState.Locked:
                    connection.SetSprite(manager.connectionLocked);
                    break;
                case BraceletUpgradeState.Unlocked:
                    connection.SetSprite(manager.connectionUnlocked);
                    break;
            }
            
            connections.Add(connection);

            StartCoroutine(upgradeButton.InitConnections());
        }
    }
    
    // For Testing Purposes (but it works)
    [ContextMenu("NotifyNeighbours")]
    public void NotifyNeighbours()
    {
        foreach (BraceletUpgradeConnection connection in connections)
        {
            if (connection.nextUpgrade.state == BraceletUpgradeState.Hidden)
            {
                connection.SetSprite(manager.connectionLocked);
                connection.nextUpgrade.state = BraceletUpgradeState.Locked;
                connection.nextUpgrade.UpdateSprite();
            }
        }
    }

    public void Upgrade()
    {
        if (!(currentLevel < maxLevel) || badgeState == BadgeState.Forbidden)
        {
            Debug.Log($"Cannot upgrade! state={state} || level={currentLevel}/{maxLevel}");
            return;
        }
        
        manager.gameManager.BraceletUpgradesHandler.Upgrade(upgrade);

        InvokeBraceletUpgrade(upgrade);
        
        currentLevel++;
        if (currentLevel == maxLevel)
        {
            badgeState = BadgeState.MaxedOut;
            UpdateBadge();
        }
        if (state == BraceletUpgradeState.Unlocked) return;
        state = BraceletUpgradeState.Unlocked;
        UpdateSprite();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.Tooltip.PassData(UpgradeName, description, cost);
        manager.Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.Tooltip.SetActive(false);

    }
    
    public static void InvokeBraceletUpgrade(BraceletUpgrades obj)
    {
        OnBraceletUpgrade?.Invoke(obj);
    }
}
