using System;
using PlayerBundle;
using UnityEngine;

namespace Enemies.EnemiesAI
{
    public class AIFlyingScript : AIDad
    {
        private Vector2 _targetPos;
        private Vector2 _previousPos;

        private Vector3 _currentPosition;
        [SerializeField]
        private float updateFrameCount;
        
        public Rigidbody2D rb;
        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("Attack");

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
            if (_enemyInstance.DmgInteractionTimer > 0)
            {
                return;
            }
            else
            {
                Debug.Log("ATTACKING FUCKER");
                _animator.SetTrigger(Attack);
                targetPlayer.Damage(_enemyInstance.attack);
                _enemyInstance.InitInteractionTimer();
            }

        }

        private void UpdatePath()
        {
            Vector2 direction = (targetPlayer.transform.position - transform.position);
            rb.velocity = direction.normalized * _enemyInstance.Speed;
            _enemyInstance.renderer.flipX = direction.x < 0;
        }
        
    }
}
