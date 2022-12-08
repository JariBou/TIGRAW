using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [Header("Movement Variables")]
    public int movespeed;
    public int sprintspeed;
    private Vector2 moveVector;
    bool running;
    public Vector3 Position ;


    [Header("Dash Variables")]
    public int dashDistance;

    [HideInInspector]
    public bool isDashing;

    [Header("Teleportation Variables")]
    private Vector3 mousePos;

    public bool isTeleporting = false;


    [Header("Other")]
    public GameObject cameraObj;
    private Rigidbody2D body;
    private CircleCollider2D circleCollider;
    private PlayerActions playerActions;

    [HideInInspector]
    public Vector3 circleColliderOffset;

    [HideInInspector]
    public SpriteRenderer sprite;
    private CameraFollow cameraScript;
    private Animator animator;
    public static Player instance;
    
    // For TP and checking if in bounds
    public CompositeCollider2D floor;

    [Header("Heat Mechanic")]
    public float heatRecoveryRate = 2f;
    [SerializeField] public float heatAmount = 0;
    private float timer = 0;
    private bool isCasting = false;
    private static readonly int Facing = Animator.StringToHash("facing");
    private BraceletUpgrades _braceletUpgrades;
    
    void Awake() {
        instance = this;
        
        playerActions = new PlayerActions();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        cameraScript = cameraObj.GetComponent<CameraFollow>();
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

    public Vector2 GetMoveDirection()
    {
        return moveVector;
    }
    
    // Update is called once per frame
    void Update()
    {
        running = false;
        
        if (Input.GetKey(KeyCode.LeftShift)) {
            running = true;
        } 
    }

    public void Move(InputAction.CallbackContext context) {

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
        return;

        isDashing = true;

        heatAmount += 10;

        Debug.Log($"{moveVector.x}, {moveVector.y}");

        body.velocity = new Vector2(moveVector.x, moveVector.y ) * dashDistance * Time.fixedDeltaTime;

        Invoke("EndDash", 0.25f);
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
    
    
    // Work on Casting spells by type:
    public void CastSpell(InputAction.CallbackContext context, int spellId)
    {
        if (isTeleporting) return;
        if (!context.performed) return;
        
        ResetTimer();
        
        GameObject spellPrefab = SpellsList.getSpell(spellId);
        playerSpell spellPrefabScript = spellPrefab.GetComponent<playerSpell>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
        if (spellPrefabScript.isProjectile)
        {
            Vector2 direction = (mousePos - transform.position);
            direction /= direction.magnitude;

            GameObject spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
            playerSpell spellScript = spell.GetComponent<playerSpell>();

            spellScript.SetDirection(direction);
            heatAmount += spellScript.getHeat();
        }
        else
        {
            GameObject spell = Instantiate(spellPrefab, mousePos, Quaternion.identity);
            playerSpell spellScript = spell.GetComponent<playerSpell>();

            spellScript.SetPosition(mousePos);
            heatAmount += spellScript.getHeat();
        }
    }


    public void SetVelocity(Vector2 vector)
    {
        body.velocity = vector;
    }

    public void ApplyForce(Vector2 testVec)
    {
        body.AddForce(testVec, ForceMode2D.Impulse);
    }
}
