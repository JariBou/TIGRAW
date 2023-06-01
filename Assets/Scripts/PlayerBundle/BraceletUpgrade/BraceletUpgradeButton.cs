using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PlayerBundle.BraceletUpgrade
{
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
        private GameManager _gm;
        public Image badgeImage;

        public BraceletUpgradeState state = BraceletUpgradeState.Hidden;
        public BadgeState badgeState = BadgeState.Neutral;

        public BraceletUpgradeManager manager;

        public BraceletUpgradeButton[] nextUpgrades;
        public List<BraceletUpgradeConnection> connections = new(8);
        public List<BraceletUpgradeConnection> prevConnections = new(8);
    
        public List<BraceletUpgrades> IncompatibleUpgrades;
    
        public static event Action<BraceletUpgrades> OnBraceletUpgrade;
        public static event Action<BraceletUpgrades, bool> OnBraceletHover;



        private void Awake()
        {
            _buttonImage = GetComponent<Button>().image;
            _rectTransform = GetComponent<RectTransform>();
            _gm = manager.gameManager;
            _gm.BraceletUpgradesHandler.UpgradesAmount[upgrade] = upgradeAmount;
            GetComponent<Button>().onClick.RemoveListener(Upgrade);

        }

        private void Start()
        {
            OnBraceletUpgrade += OnOtherBraceletUpgrade;
            OnBraceletHover += OnOtherBraceletHover;
            
            GetComponent<Button>().onClick.AddListener(Upgrade);
        }

        private void OnDisable()
        {
            OnBraceletUpgrade -= OnOtherBraceletUpgrade;
            OnBraceletHover -= OnOtherBraceletHover;
        }

        private void OnOtherBraceletHover(BraceletUpgrades upgrade, bool enter)
        {
            if (IncompatibleUpgrades.Contains(upgrade))
            {
                _buttonImage.color = enter ? Color.red : Color.white;
            }
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

        public void SetInitialState(bool isFirst = false)
        {
            //BraceletUpgradesHandler handler = manager.gameManager.BraceletUpgradesHandler;
            currentLevel = manager.gameManager.BraceletUpgradesHandler.UpgradesLvl[upgrade];
            manager.gameManager.BraceletUpgradesHandler.UpgradesAmount[upgrade] = upgradeAmount;
            badgeState = BadgeState.Neutral;
            
            connections = new List<BraceletUpgradeConnection>(8);
            
            prevConnections = new List<BraceletUpgradeConnection>(8);
            
            if (currentLevel > 0)
            {
                state = BraceletUpgradeState.Unlocked;
                InvokeBraceletUpgrade(upgrade);
                if (currentLevel == maxLevel)
                {
                    badgeState = BadgeState.MaxedOut;
                }
            }
            else
            {
                state = isFirst ? BraceletUpgradeState.Locked : BraceletUpgradeState.Hidden;
                
                foreach (BraceletUpgrades upgrades in IncompatibleUpgrades)
                {
                    if (manager.gameManager.BraceletUpgradesHandler.GetUpgradedAmount(upgrades) > 0)
                    {
                        badgeState = BadgeState.Forbidden;
                        break;
                    }
                }
            }
            UpdateBadge();
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
                
                upgradeButton.SetInitialState();

                if (state == BraceletUpgradeState.Unlocked && upgradeButton.state == BraceletUpgradeState.Hidden)
                {
                    upgradeButton.state = BraceletUpgradeState.Locked;
                }
    
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
                upgradeButton.prevConnections.Add(connection);

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
            
            if(state == BraceletUpgradeState.Hidden) {return;}

            if (_gm.currentSave.playerSouls < cost)
            {
                return;
            }


            _gm.currentSave.playerSouls -= cost;
            _gm.BraceletUpgradesHandler.Upgrade(upgrade);
            NotifyNeighbours();

            foreach (BraceletUpgradeConnection prevConnection in prevConnections)
            {
                prevConnection.SetSprite(manager.connectionUnlocked);
            }
            InvokeBraceletUpgrade(upgrade);

            currentLevel++;
            if (currentLevel == maxLevel)
            {
                badgeState = BadgeState.MaxedOut;
                UpdateBadge();
            }
            manager.Tooltip.PassData(UpgradeName, description, $"Cost: {cost}  |  {currentLevel}/{maxLevel}");
            if (state == BraceletUpgradeState.Unlocked) return;
            state = BraceletUpgradeState.Unlocked;
            UpdateSprite();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            manager.Tooltip.PassData(UpgradeName, description, $"Cost: {cost}  |  {currentLevel}/{maxLevel}");
            manager.Tooltip.SetActive(true);
            InvokeBraceletHover(upgrade, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            manager.Tooltip.SetActive(false);
            InvokeBraceletHover(upgrade, false);
        }
    
        public static void InvokeBraceletUpgrade(BraceletUpgrades obj)
        {
            OnBraceletUpgrade?.Invoke(obj);
        }
        
        public static void InvokeBraceletHover(BraceletUpgrades obj, bool enter)
        {
            OnBraceletHover?.Invoke(obj, enter);
        }

        private void OnValidate()
        {
            gameObject.name = UpgradeName;
        }
    }
}