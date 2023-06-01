using System;
using PlayerBundle;
using UnityEngine;

namespace Enemies.EnemiesAI
{
    public class AIFlyingKamikaze: AIDad
    {
        private Vector2 _targetPos;
        private Vector2 _previousPos;

        private Vector3 _currentPosition;
        [SerializeField]
        private float updateFrameCount;

        public GameObject explosionEntity;
        
        public Rigidbody2D rb;
        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("Attack");
        
        private Collider2D[] _results = new Collider2D[32];

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _enemyInstance = GetComponent<EnemyInterface>();
            targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _animator = _enemyInstance.animator;
            var position = transform.position;
            _targetPos = position;
            _previousPos = position;
        }
    
        void Update()
        {
            if (_enemyInstance.isInRange) {     // Leave it here because ranged and melee will not behave the same       
                _previousPos = transform.position;
                
                
                AttackPlayer();
                
                return;
            }
            updateFrameCount += 1;
            if (updateFrameCount >= updateRate)
            {
                UpdatePath();
                updateFrameCount = 0;
            }
        }

        private void AttackPlayer()
        {
            Debug.Log("ATTACKJIIIGIIHGIVE");
            Instantiate(explosionEntity, transform.position, Quaternion.identity);
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, GetComponent<CircleCollider2D>().radius*2, _results);

            for (int i = 0; i < size; i++)
            {
                try
                {
                    if (_results[i].isTrigger) {continue;}

                    if (_results[i].CompareTag("Player"))
                    {
                        _results[i].GetComponent<Player>().Damage(_enemyInstance.attack);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"PROBLEM WITH {_results[i]} Stacktrace: {e}");
                }
            }
            Destroy(gameObject);
        }
        
        private void UpdatePath()
        {
            Vector2 direction = (targetPlayer.transform.position - transform.position);
            rb.velocity = direction.normalized * _enemyInstance.Speed;
            _enemyInstance.renderer.flipX = direction.x < 0;
        }
        
    }
}