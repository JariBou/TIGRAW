using System.Collections;
using System.Collections.Generic;
using PlayerBundle.BraceletUpgrade;
using PlayerBundle.PlayerUpgrades;
using Spells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

//TODO: Put Every bracelet mechanic related stuff in another script
//TODO: Put Everything related to the pause menu in another script

namespace PlayerBundle
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [FormerlySerializedAs("movespeed")]
        [Header("Movement Variables")]
        [SaveVariable] public float baseMovespeed; // Modified by bracelet potentially
        [SaveVariable] public float movespeed; 
        [SaveVariable] public bool canSprint; // Modified by bracelet potentially
        private Vector2 moveVector;
        [HideInInspector]
        public bool running;
        [HideInInspector] public bool isDashing;
        

        [Header("Stats Variables")] 
        [FormerlySerializedAs("atkMultiplier")] [SaveVariable] public float baseAtkMultiplier = 0.9f; // Modified by Upgrades
        public float health => playerData.health;  // Only variable that needs to be passed between scenes but doesn't get saved :sob:
        [FormerlySerializedAs("armor")] [SaveVariable] public int baseArmor; // Modified by Upgrades

        private PlayerData playerData => _gm.playerData;

        // public List<StatusEffect> Statuses;

        // For some reason those donc work... thanks... nvm they work... Sometimes?
        public float AtkMultiplier =>
            baseAtkMultiplier +
            _gm.PlayerUpgradesHandler.GetUpgradedAmount(PlayerUpgrades.PlayerUpgrades.AtkMultiplier) +
            _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.AttackMultiplierIncrease) +
            _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.BonusDmgOnHeatLvl) * heatManager.GetHeatLvl();

        public float MaxHealth =>
            playerData.MaxHealth +
            _gm.PlayerUpgradesHandler.GetUpgradedAmount(PlayerUpgrades.PlayerUpgrades.Health) +
            _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.HealthIncrease);


        [Header("Teleportation Variables")]
        private Vector3 mousePos;
        public bool isTeleporting;
        public Tilemap groundTilemap;
        public LayerMask interactableLayers;
        
        
        [HideInInspector]
        public Canvas pauseMenuCanvas;
        [Header("Menus")]
        public GameObject pauseMenuPrefab;
        public bool gamePaused;
        public StatsWindow.StatsWindow playerStatsScript;
        

        [Header("Other")]
        public GameObject cameraObj;
        private Rigidbody2D body;
        private CircleCollider2D circleCollider;
        private PlayerActions playerActions;
        [HideInInspector] public float interactionRange;
        [HideInInspector] public Vector3 circleColliderOffset;
        [FormerlySerializedAs("sprite")] [HideInInspector] public SpriteRenderer spriteRenderer;
        public Animator animator;
        public readonly int Facing = Animator.StringToHash("facing");
        
        // ============================
        public static Player Instance;
        // ============================

        [FormerlySerializedAs("heatRecoveryRate")]
        // [Header("Heat Mechanic")]
        public HeatManager heatManager;
        
        private bool _isCasting = false;

        private GameManager _gm;
        
        [SaveVariable]
        private BraceletUpgrades _braceletUpgrades;
        [SaveVariable]
        private PlayerUpgradesHandler _playerUpgradesHandler;

        private float dmgTakenMultiplier = 1f;

        public List<StatusEffect> StatusEffects = new (8);
        public float OverdriveMultiplier = 1f;


        void Awake() {
            Instance = this;
            enabled = true;
            movespeed = baseMovespeed;

            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            playerActions = new PlayerActions();
            body = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();
            heatManager = GetComponent<HeatManager>();
            circleColliderOffset = new Vector3(circleCollider.offset.x, circleCollider.offset.y, 0);
            pauseMenuCanvas = Instantiate(pauseMenuPrefab).GetComponent<Canvas>();

            // _playerUpgradesHandler = new PlayerUpgradesHandler();
            // _braceletUpgrades = new BraceletUpgrades();

        }

        // Used to get position of feet, to do logic relative to path finding or player position
        // (the player position is defined at it's feet)
        public Vector3 GetPosition()
        {
            return transform.position + circleColliderOffset;
        }

        public float GetMaxHealth()
        {
            return playerData.MaxHealth +
                   _gm.PlayerUpgradesHandler.GetUpgradedAmount(PlayerUpgrades.PlayerUpgrades.Health) +
                   _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.HealthIncrease);
        }

        // <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            playerActions.Playermaps.Enable();
        }

        private void OnDisable()
        {
            playerActions.Playermaps.Disable();
        }

        // public float GetAtkMultiplier()
        // {
        //     Debug.Log($"BraceleUpgrades: {_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.AttackMultiplierIncrease)}");
        //     Debug.Log($"BraceleUpgrades2: {_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.BonusDmgOnHeatLvl) * heatManager.GetHeatLvl()}");
        //     return baseAtkMultiplier +
        //         _gm.PlayerUpgradesHandler.GetUpgradedAmount(PlayerUpgrades.PlayerUpgrades.AtkMultiplier) +
        //         _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.AttackMultiplierIncrease) +
        //         _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.BonusDmgOnHeatLvl) * heatManager.GetHeatLvl();
        // }

        public void Damage(float amount)
        {
            if (isTeleporting) {return;} // if player is teleporting lets give him Iframes for now, might remove it still tho
            if (isDashing && _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.IFramesWhileDashing) > 0) {return;}
            playerData.health -= amount * dmgTakenMultiplier;
            _gm.currentRunData.playerHealth = playerData.health; // Should be useless

            StartCoroutine(TakeDmgAnim());

        }
        
        void ResolveStatusEffects()
        {
            foreach (var statusEffect in StatusEffects)
            {
                StatusType type = statusEffect.StatusType;
                if (statusEffect.IsActive())
                {
                    if (type == StatusType.Slow)
                    {
                        movespeed = baseMovespeed * statusEffect.value;
                    } else if (type == StatusType.DmgTakenIncrease)
                    {
                        dmgTakenMultiplier = statusEffect.value;
                    } else if (type == StatusType.HeatReductionRateDecrease)
                    {
                        OverdriveMultiplier = statusEffect.value;
                    }
                }
                if (!statusEffect.IsActive())
                {
                    if (type == StatusType.Slow)
                    {
                        movespeed = baseMovespeed;
                    } else if (type == StatusType.DmgTakenIncrease)
                    {
                        dmgTakenMultiplier = 1f;
                    }  else if (type == StatusType.HeatReductionRateDecrease)
                    {
                        OverdriveMultiplier = 1f;
                    }
                }
            }
        }

        private IEnumerator TakeDmgAnim()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }

        public void Heal(float amount)
        {
            playerData.health += amount;
            _gm.currentRunData.playerHealth = health;
        }
        
        public void SetMoveVector(Vector2 vector)
        {
            moveVector = vector;
        }

        public bool IsPerformingAction()
        {
            if (_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.IFramesWhileDashing) > 0)
            {
                return isTeleporting;
            }
            return isTeleporting || isDashing;

            // return isTeleporting;
        }

        private void FixedUpdate()
        {
            if (playerData.health <= 0)
            {
                Kill();
                return;
            }


            // foreach (var statusEffect in Statuses)
            // {
            //     playerData.health -= statusEffect.Tick();
            //     if (statusEffect.Duration <= 0)
            //     {
            //         Statuses.Remove(statusEffect);
            //     }
            // }
            
            // Debug.Log($"Movespeed: {movespeed}");
            
            ResolveStatusEffects();
            if (isTeleporting || isDashing) {return;}
            body.velocity = new Vector2(moveVector.x, moveVector.y) * (movespeed * Time.fixedDeltaTime);
            
            
            
        }

        public Vector2 GetMoveVector() {
            return moveVector;
        }

        public void SetVelocity(Vector2 vector)
        {
            body.velocity = vector;
        }

        public void ApplyForce(Vector2 testVec)
        {
            body.AddForce(testVec, ForceMode2D.Impulse);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0f, -0.88f, 0f), interactionRange);
        }

        public void Kill()
        {
            EventManager.InvokeFlagEvent(Flag.PlayerDeath);
        }
    }
}
