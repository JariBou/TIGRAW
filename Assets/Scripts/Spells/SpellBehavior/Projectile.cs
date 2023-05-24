using System;
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
        
            spell.player.heatManager.AddHeat(spell.heatProduction);

            if (spell.destroyOnAnimEnd)
            {
                StartCoroutine(DelayedDestroy(spell.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
            }

        
        
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
    
        private void OnTriggerEnter2D(Collider2D col)
        {
        
            if(_hasCollided) {return;}

            EnemyInterface enemyScript = null;
            if (col.CompareTag("Enemy"))
            {
                // Debug.Log("HIT ENNEMY!");
                enemyScript = col.GetComponent<EnemyInterface>();
            }

            if (col.isTrigger)
            {
                if (col.CompareTag("Enemy Hitbox"))
                {
                    Debug.Log("Enemy Additional Hitbox");
                    enemyScript = col.GetComponentInParent<EnemyInterface>();
                }
                else
                {
                    return;
                }
            }

            if (enemyScript == null)
            {
                return;
            }
        
        
            if (col.CompareTag("Enemy") || col.CompareTag("Enemy Hitbox"))
            {
                // Debug.Log("HIT ENNEMY!");
                if (collidedEnnemiesId.Contains(enemyScript.id))
                {
                    return;
                }

                collidedEnnemiesId.Add(enemyScript.id);
                StartCoroutine(DelayedRemoval(spell.interactionInterval, enemyScript.id));
                enemyScript.Damage(spell.Damage);
                spell.ApplyStatus(enemyScript);
                if (spell.hasOnHitEffect)
                {
                    Instantiate(spell.onHitEffect, transform.position, transform.rotation);
                }
                if (!spell.isInfPierce)
                {
                    _hasCollided = true;
                    
                    Destroy(gameObject, 0.1f); // Actually only destroy if hasCollided

                }
            } else if (col.CompareTag("Wall") )
            {
                // Debug.Log("Hits wall!!!");
                if (!spell.phantom)
                {
                    if (spell.hasOnHitEffect)
                    {
                        Instantiate(spell.onHitEffect, transform.position, transform.rotation);
                    }
                    // Debug.Log("No phantom fucker");
                    _hasCollided = true;
                    Destroy(gameObject, 0.01f); // Actually only destroy if hasCollided
                }
            }
        
        
        }

        // TODO: maybe don't use a coroutine for that, and do a timer in an Update function
        IEnumerator DelayedRemoval(float delay, int id)
        {
            yield return new WaitForSeconds(delay);
            collidedEnnemiesId.Remove(id);
        }
        
        IEnumerator DelayedDestroy(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    
    }
}
