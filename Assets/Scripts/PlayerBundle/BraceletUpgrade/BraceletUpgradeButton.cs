using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public BraceletUpgradeConnection prevConnection;
    
        public List<BraceletUpgrades> IncompatibleUpgrades;
    
        public static event Action<BraceletUpgrades> OnBraceletUpgrade;



        private void Awake()
        {
            _buttonImage = GetComponent<Button>().image;
            _rectTransform = GetComponent<RectTransform>();
            _gm = manager.gameManager;
            GetComponent<Button>().onClick.RemoveListener(Upgrade);

        }

        private void Start()
        {
            OnBraceletUpgrade += OnOtherBraceletUpgrade;
            
            GetComponent<Button>().onClick.AddListener(Upgrade);

            _gm.BraceletUpgradesHandler.UpgradesAmount[upgrade] = upgradeAmount;
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

        public void SetInitialState()
        {
            BraceletUpgradesHandler handler = manager.gameManager.BraceletUpgradesHandler;
            upgradeAmount = handler.UpgradesAmount[upgrade];
            currentLevel = handler.UpgradesLvl[upgrade];

            if (currentLevel > 0)
            {
                state = BraceletUpgradeState.Unlocked;
                InvokeBraceletUpgrade(upgrade);
                if (currentLevel == maxLevel)
                {
                    badgeState = BadgeState.MaxedOut;
                }
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
                upgradeButton.prevConnection = connection;

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

            if (_gm.currentSave.playerSouls < cost)
            {
                return;
            }


            _gm.currentSave.playerSouls -= cost;
            _gm.BraceletUpgradesHandler.Upgrade(upgrade);
            InvokeBraceletUpgrade(upgrade);
            NotifyNeighbours();

            if (prevConnection != null)
            {
                Debug.Log("Changing prev connection sprite");
                prevConnection.SetSprite(manager.connectionUnlocked);
            }
        
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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            manager.Tooltip.SetActive(false);

        }
    
        public static void InvokeBraceletUpgrade(BraceletUpgrades obj)
        {
            OnBraceletUpgrade?.Invoke(obj);
        }

        private void OnValidate()
        {
            gameObject.name = UpgradeName;
        }
    }
}