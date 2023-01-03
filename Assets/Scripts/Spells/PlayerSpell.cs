using Enemies.EnemyScript;
using UnityEngine;

namespace Spells
{
    public class PlayerSpell : MonoBehaviour
    {
        public float projectileSpeed = 2f;
        public float damage = 10;
        public int id;
        public bool isProjectile = true;
        public bool isAOE;
        public float heatProduction = 2f;

        private Vector2 direction = Vector2.zero;

        private bool hasCollided;
        // Start is called before the first frame update
        void Start()
        {
            if (direction != Vector2.zero) {return;}
            direction = Vector2.up;
            if (!isProjectile){
                Destroy(gameObject, projectileSpeed);
            }

        }

        public void SetDirection(Vector2 direction) {
            this.direction = direction;
        }

        public void SetPosition(Vector2 position) {
            transform.position = position;
        }

        public float getHeat() {
            return heatProduction;
        }
    
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (hasCollided)
            {
                return;
            
            }
            if (collider.CompareTag("Enemy") )
            {
                if (!isAOE)
                {
                    hasCollided = true;
                }
                collider.GetComponent<MeleeEnemyScript>().Damage(damage);
                Debug.Log("HIT ENNEMY!");
                Destroy(gameObject, 0.1f);

            }
            if (!collider.CompareTag("Player") ) {
                Debug.Log("HIT!");
                Destroy(gameObject, 0.1f);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isProjectile){
                transform.position = new Vector2(transform.position.x, transform.position.y) + direction * (projectileSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
