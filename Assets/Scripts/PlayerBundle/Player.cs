using PlayerBundle.BraceletUpgrade;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

//TODO: Put Every bracelet mechanic related stuff in another script
//TODO: Put Everything related to the pause menu in another script

namespace PlayerBundle
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [Header("Movement Variables")]
        public int movespeed;
        public int sprintspeed;
        private Vector2 moveVector;
        bool running;

        [Header("Entity Variables")] 
        public float health = 50f;
        public float atkMultiplier = 0.9f;
        public int armor;
    

        [HideInInspector]
        public bool isDashing;

        [Header("Teleportation Variables")]
        private Vector3 mousePos;

        public bool isTeleporting;


        [Header("Other")]
        public GameObject cameraObj;
        private Rigidbody2D body;
        private CircleCollider2D circleCollider;
        private PlayerActions playerActions;
        [HideInInspector]
        public bool gamePaused;

        public LayerMask interactableLayers;
        public Canvas pauseMenuCanvas;


        [SerializeField] public float interactionRange;
        public Tilemap groundTilemap;

        [HideInInspector]
        public Vector3 circleColliderOffset;

        public StatsWindow.StatsWindow playerStatsScript;

        [HideInInspector]
        public SpriteRenderer sprite;
        public Animator animator { get; set; }
        public static Player instance;

        [Header("Heat Mechanic")]
        [SaveVariable]
        public float heatRecoveryRate = 2f;
        public float heatAmount;
        [SaveVariable]
        public float maxHeat = 100f;        // SAVE
        [SerializeField]
        private bool infiniteHeat;
        private float timer;
        private bool isCasting = false;
        public readonly int Facing = Animator.StringToHash("facing");
        private BraceletUpgrades _braceletUpgrades;
    
        void Awake() {
            Debug.Log("AWAKENING PLAYER");
            instance = this;
        
            playerActions = new PlayerActions();
            body = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();
            circleColliderOffset = new Vector3(circleCollider.offset.x, circleCollider.offset.y, 0);

            _braceletUpgrades = new BraceletUpgrades();
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

        // Start is called before the first frame update
        void Start()
        {

        }


        public void SetMoveVector(Vector2 vector)
        {
            moveVector = vector;
        }
    
        // Update is called once per frame
        void Update()
        {
            if (infiniteHeat)
            {
                heatAmount = 0;
            }
        
            running = false;
        
            if (Input.GetKey(KeyCode.LeftShift)) {
                running = true;
            } 
        }

        private bool IsPerformingAction() {
            return isTeleporting || isDashing;
            // return isTeleporting;
        }

        private void FixedUpdate()
        {

            if (heatAmount > 0) heatAmount -= heatRecoveryRate * Time.fixedDeltaTime;
            else if (heatAmount < 0) heatAmount = 0;

            if (IsPerformingAction()) {ResetTimer(); return;}
        
            timer += 1f * Time.fixedDeltaTime;

            if (timer >= 3) {
                if (heatAmount > 0) heatAmount -= heatRecoveryRate * Time.fixedDeltaTime * 2;
            }

            body.velocity = new Vector2(moveVector.x, moveVector.y) * ((running ? sprintspeed : movespeed) * Time.fixedDeltaTime);
        }

        public Vector2 GetMoveVector() {
            return moveVector;
        }

        public int GetHeat() {
            return (int) heatAmount;
        }

        public void ResetTimer() {
            timer = 0;
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
