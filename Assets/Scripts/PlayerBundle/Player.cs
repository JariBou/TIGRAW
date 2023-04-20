using System;
using System.Collections;
using PlayerBundle.BraceletUpgrade;
using PlayerBundle.PlayerUpgrades;
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
        [SaveVariable] public int baseMovespeed; // Modified by bracelet potentially
        [FormerlySerializedAs("sprintspeed")] [SaveVariable] public int baseSprintspeed; // Modified by bracelet potentially
        [SaveVariable] public bool canSprint; // Modified by bracelet potentially
        private Vector2 moveVector;
        [HideInInspector]
        public bool running;
        [HideInInspector] public bool isDashing;
        

        [Header("Stats Variables")] 
        public float health = 50f;  // Only variable that needs to be passed between scenes but doesn't get saved :sob:
        [FormerlySerializedAs("maxHealth")] [SaveVariable] public float baseMaxHealth = 50f; // Modified by Upgrades
        [FormerlySerializedAs("atkMultiplier")] [SaveVariable] public float baseAtkMultiplier = 0.9f; // Modified by Upgrades
        [FormerlySerializedAs("armor")] [SaveVariable] public int baseArmor; // Modified by Upgrades

        public float AtkMultiplier =>
            baseAtkMultiplier +
            _gm.PlayerUpgradesHandler.GetUpgradedAmount(PlayerUpgrades.PlayerUpgrades.AtkMultiplier);

        public float MaxHealth =>
            baseMaxHealth +
            _gm.PlayerUpgradesHandler.GetUpgradedAmount(PlayerUpgrades.PlayerUpgrades.Health);


        [Header("Teleportation Variables")]
        private Vector3 mousePos;
        public bool isTeleporting;
        public Tilemap groundTilemap;
        public LayerMask interactableLayers;
        
        
        [Header("Menus")]
        public Canvas pauseMenuCanvas;
        public bool gamePaused;
        public StatsWindow.StatsWindow playerStatsScript;
        

        [Header("Other")]
        public GameObject cameraObj;
        private Rigidbody2D body;
        private CircleCollider2D circleCollider;
        private PlayerActions playerActions;
        [HideInInspector] public float interactionRange;
        [HideInInspector] public Vector3 circleColliderOffset;
        [HideInInspector] public SpriteRenderer sprite;
        public Animator animator { get; set; }
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
        
    
        void Awake() {
            Debug.Log("AWAKENING PLAYER");
            Instance = this;
            enabled = true;

            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            playerActions = new PlayerActions();
            body = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();
            heatManager = GetComponent<HeatManager>();
            circleColliderOffset = new Vector3(circleCollider.offset.x, circleCollider.offset.y, 0);
            
            // _playerUpgradesHandler = new PlayerUpgradesHandler();
            // _braceletUpgrades = new BraceletUpgrades();

        }

        // Used to get position of feet, to do logic relative to path finding or player position
        // (the player position is defined at it's feet)
        public Vector3 GetPosition()
        {
            return transform.position + circleColliderOffset;
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

        public void Damage(float amount)
        {
            health -= amount;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }


        public void SetMoveVector(Vector2 vector)
        {
            moveVector = vector;
        }

        public bool IsPerformingAction() {
            return isTeleporting || isDashing;
            // return isTeleporting;
        }

        private void FixedUpdate()
        {
            
            body.velocity = new Vector2(moveVector.x, moveVector.y) * ((running ? baseSprintspeed : baseMovespeed) * Time.fixedDeltaTime);
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
    }
}
