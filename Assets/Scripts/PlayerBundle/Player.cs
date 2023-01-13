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
        public float heatRecoveryRate = 2f;
        public float heatAmount;
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

        public void Move(InputAction.CallbackContext context) 
        {
            moveVector = context.ReadValue<Vector2>().normalized;
    
            if ((int)moveVector.x == 1 && (int)moveVector.y == 0)
            {
                animator.SetInteger(Facing, 3);
            } else if ((int)moveVector.x == -1 && (int)moveVector.y == 0)
            {
                animator.SetInteger(Facing, 9);
            } else if (Mathf.Abs((int)moveVector.y) == 1)
            {
                animator.SetInteger(Facing, 9 + 3*(int)moveVector.y);
            } 
        

            animator.SetFloat("xMovement", moveVector.x);
            animator.SetFloat("yMovement", moveVector.y);
            animator.SetFloat("speed", moveVector.sqrMagnitude);

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
    
        public void Dash(InputAction.CallbackContext context) {
            if (heatAmount > 100) {return;}
            if (isTeleporting) {return;}
            if (isDashing) {return;}
            if (moveVector.magnitude < 0.01) {return;}
    
        
            SpellCasting.CastSpell(context, 4);
        }

        public void EndDash() {
            isDashing = false;
        }
    
        public Vector2 GetMoveVector() {
            return moveVector;
        }

        public void Teleport(InputAction.CallbackContext context) {
            if (heatAmount > 100) {return;}
            if (isTeleporting) {return;}
            if (!context.performed){return;}
     
            SpellCasting.CastSpell(context, 3);
        }

        public void DoBasicAttack(InputAction.CallbackContext context) {
            if (isTeleporting) {return;}
            if (!context.performed){return;}

            SpellCasting.CastSpell(context, 2);
        }

        public void DoThunderStrike(InputAction.CallbackContext context) {
            if (isTeleporting) {return;}
            if (!context.performed){return;}

            SpellCasting.CastSpell(context, 1);
        }

        public int GetHeat() {
            return (int) heatAmount;
        }

        public void ResetTimer() {
            timer = 0;
        }

        public void PauseGame(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (gamePaused)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }

            gamePaused = !gamePaused;
            pauseMenuCanvas.enabled = gamePaused;
            moveVector = Vector2.zero;
        }


        public void SetVelocity(Vector2 vector)
        {
            body.velocity = vector;
        }

        public void ApplyForce(Vector2 testVec)
        {
            body.AddForce(testVec, ForceMode2D.Impulse);
        }

        public void TryInteracting(InputAction.CallbackContext context)
        {
            if (!context.performed) {return;}
        
            Collider2D[] results = Physics2D.OverlapCircleAll(GetPosition(), interactionRange, interactableLayers);

            foreach (Collider2D col in results)
            {
                Debug.LogWarning(col.name);
                if (col.CompareTag("Interactable"))
                {
                    col.GetComponent<Interactable>().Interact();
                }
            }
        }

        public void ShowStats(InputAction.CallbackContext context)
        {
            if(context.performed) // the key has been pressed
            {
                playerStatsScript.Enable();
            }
            if(context.canceled) //the key has been released
            {
                playerStatsScript.Disable();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0f, -0.88f, 0f), interactionRange);
        }
    }
}
