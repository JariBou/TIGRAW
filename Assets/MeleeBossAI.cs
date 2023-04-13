using System.Collections;
using System.Collections.Generic;
using Enemies;
using PathFinding;
using PlayerBundle;
using UnityEngine;

public class MeleeBossAI : EnemyInterface
{

    public CircleCollider2D playerDetection;
    
    public float speed = 2f;
    public GameObject targetEntity;

    private EnemyInterface _enemyInstance;
    private Animator _animator;
    private SpriteRenderer _renderer;
        
    public Rigidbody2D rb;
    private static readonly int Attack = Animator.StringToHash("Attack");

    [Header("Particle Effect")] 
    public GameObject particle;
    public Vector2 particlePosition;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        targetEntity = Player.Instance.gameObject;
        self = gameObject;
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        Instance = this;
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
        _enemyInstance = GetComponent<EnemyInterface>();
    }


    private new void FixedUpdate()
    {

        if (health <= 2f / 3f * MaxHealth)
        {
            if (timer <= 0)
            {
                Instantiate(particle, transform.position+new Vector3(particlePosition.x * (_renderer.flipX ? -1 : 1), particlePosition.y), Quaternion.identity, transform);
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

        _renderer.flipX = direction.x < 0;
        
        rb.velocity = direction * (speed * Time.fixedDeltaTime);
        
        if (isInRange)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (_enemyInstance.DmgInteractionTimer > 0)
        {
            return;
        }
        else
        {
            Debug.Log("ATTACKING FUCKER");
            
            _animator.SetTrigger(Attack);

            Player.Instance.Damage(_enemyInstance.attack);
            _enemyInstance.InitInteractionTimer();
        }

    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + (Vector3)particlePosition, 0.5f);
    }
    
}
