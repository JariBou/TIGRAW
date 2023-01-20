using System.Collections;
using System.Collections.Generic;
using Enemies;
using PlayerBundle;
using UnityEngine;

namespace Spells.SpellBehavior
{
    public class Projectile : MonoBehaviour
    {
        public Spell spell;

        private bool _hasCollided;

        public List<int> collidedEnnemiesId;

        // Start is called before the first frame update
        void Start()
        {
            collidedEnnemiesId = new List<int>();
        
        
            Player.instance.heatAmount += spell.heatProduction;

        
        
            gameObject.transform.position += new Vector3(spell.Direction.x, spell.Direction.y, 0); // Offset so it casts a bit in front of you
            // That way you can cast while next to a wall lol
            //TODO: You can cast through a small enough wall, fix it 
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var transform1 = transform;
            Vector3 transformVector = transform1.position;
            transform1.position = new Vector2(transformVector.x, transformVector.y) + spell.Direction * (spell.projectileSpeed * Time.fixedDeltaTime);
        }
    
        private void OnTriggerEnter2D(Collider2D collider)
        {
        
            if(_hasCollided) {return;}
        
        
            if (collider.CompareTag("Enemy") )
            {
                // Debug.Log("HIT ENNEMY!");
                EnemyInterface enemyScript = collider.GetComponent<EnemyInterface>();
                if (collidedEnnemiesId.Contains(enemyScript.id))
                {
                    return;
                }

                collidedEnnemiesId.Add(enemyScript.id);
                StartCoroutine(DelayedRemoval(spell.interactionInterval, enemyScript.id));
                enemyScript.Damage(spell.damage);
                if (!spell.isInfPierce)
                {
                    _hasCollided = true;
                    Destroy(gameObject, 0.1f); // Actually only destroy if hasCollided

                }
            } else if (collider.CompareTag("Wall") )
            {
                // Debug.Log("Hits wall!!!");
                if (!spell.phantom)
                {
                    // Debug.Log("No phantom fucker");
                    _hasCollided = true;
                    Destroy(gameObject, 0.1f); // Actually only destroy if hasCollided
                }
            }
        
        
        }
    
        IEnumerator DelayedRemoval(float delay, int id)
        {
            yield return new WaitForSeconds(delay);
            collidedEnnemiesId.Remove(id);
        }
    
    }
}
