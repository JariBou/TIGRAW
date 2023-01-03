using Enemies.EnemyScript;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spells
{
    public class PlayerSpell : MonoBehaviour
    {
        public float projectileSpeed = 2f;
        public float damage = 10;
        public int id;
        public bool isProjectile = true;
        [FormerlySerializedAs("isAOE")] public bool isAoe;
        public float heatProduction = 2f;

        private Vector2 _direction = Vector2.zero;

        private bool _hasCollided;
        // Start is called before the first frame update
        void Start()
        {
            if (_direction != Vector2.zero) {return;}
            _direction = Vector2.up;
            if (!isProjectile){
                Destroy(gameObject, projectileSpeed);
            }

        }

        public void SetDirection(Vector2 direction) {
            this._direction = direction;
        }

        public void SetPosition(Vector2 position) {
            transform.position = position;
        }

        public float GetHeat() {
            return heatProduction;
        }
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_hasCollided)
            {
                return;
            
            }
            if (col.CompareTag("Enemy") )
            {
                if (!isAoe)
                {
                    _hasCollided = true;
                }
                col.GetComponent<MeleeEnemyScript>().Damage(damage);
                Debug.Log("HIT ENNEMY!");
                Destroy(gameObject, 0.1f);

            }
            if (!col.CompareTag("Player") ) {
                Debug.Log("HIT!");
                Destroy(gameObject, 0.1f);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isProjectile)
            {
                var transform1 = transform;
                var position = transform1.position;
                position = new Vector2(position.x, position.y) + _direction * (projectileSpeed * Time.fixedDeltaTime);
                transform1.position = position;
            }
        }
    }
}
