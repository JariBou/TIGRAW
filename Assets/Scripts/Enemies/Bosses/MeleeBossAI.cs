using System.Collections.Generic;
using Enemies;
using PlayerBundle;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeBossAI : EnemyInterface
{

    public CircleCollider2D playerDetection;
    
    public GameObject targetEntity;
    private Player _player;
    
    public Rigidbody2D rb;
    private static readonly int Attack = Animator.StringToHash("Attack");

    [Header("Particle Effect")] 
    public GameObject particle;
    public Vector2 particlePosition;

    [SerializeField]
    private bool canSummon;
    private float summonTimer = 2f;

    private bool canMove = true;
    [SerializeField]
    private List<GameObject> weaklings;

    private float timer;

    public float TimeToExecute = 3f;

    // Start is called before the first frame update
    void Start()
    {
        targetEntity = GameObject.FindGameObjectWithTag("Player");
        _player = targetEntity.GetComponent<Player>();
        health = MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // if (DmgInteractionTimer == 0)
            // {
            //     Debug.Log("ZAFUCK");
            //     col.GetComponent<Player>().health -= attack;
            //     DmgInteractionTimer = DmgInteractionDelay;
            // }
            isInRange = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
    
            

    private new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }


    private new void FixedUpdate()
    {
        
        if (!canMove)
        {
            return;
        }
        
        if (summonTimer > 0)
        {
            summonTimer -= Time.fixedDeltaTime;}

       

        if (health <= 2f / 3f * MaxHealth)
        {
            if (timer <= 0)
            {
                Instantiate(particle, transform.position+new Vector3(particlePosition.x * (renderer.flipX ? -1 : 1), particlePosition.y), Quaternion.identity, transform);
                timer = 1;
            }
            else
            {
                timer -= Time.fixedDeltaTime;
            }
            attack += 1 * Time.fixedDeltaTime;
            speed += 40 * Time.fixedDeltaTime;
        }
        
        base.FixedUpdate();
        Vector2 direction = (targetEntity.transform.position - transform.position);
        direction /= direction.magnitude;

        renderer.flipX = direction.x < 0;
        
        rb.velocity = direction * (speed * Time.fixedDeltaTime); // This one is actually in a fixed update so keep it like that

        if (canSummon)
        {
            if (summonTimer <= 0)
            {
                animator.SetTrigger("Summon");
                SummonWeaklings();
                summonTimer = Random.Range(7, 20);
            }
        }
        
        if (isInRange)
        {
            AttackPlayer();
        }
    }

    private void SummonWeaklings()
    {
        rb.velocity = Vector2.zero;
        foreach (var thing in weaklings)
        {
            Instantiate(thing, transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0), Quaternion.identity, transform);
        }
    }

    private void AttackPlayer()
    {
        if (DmgInteractionTimer > 0)
        {
            return;
        }
        else
        {
            Debug.Log("ATTACKING FUCKER");
            
            animator.SetTrigger(Attack);

            _player.Damage(attack);
            InitInteractionTimer();
        }

    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + (Vector3)particlePosition, 0.5f);
    }
    
}
