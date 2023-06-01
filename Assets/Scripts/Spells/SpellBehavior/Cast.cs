using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;

// Cast will be used for AOE Attacks around player AND AOE Targeted Attacks
namespace Spells.SpellBehavior
{
    public class Cast : MonoBehaviour
    {
        public Spell spell;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Collider2D[] _results = new Collider2D[32];

        public List<int> collidedEnnemiesId = new (32);


        // Start is called before the first frame update
        void Start()
        {
            spell.player.heatManager.AddHeat(spell.heatProduction);


            if (spell.zoneSpell)
            {
                StartCoroutine(DelayedDestroy(spell.spellDuration != 0 ? spell.spellDuration : spell.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
                //InvokeRepeating("DealDamage", 0, spell.interactionInterval); //TODO: doesn't seem right
            }
            else
            {
                StartCoroutine(DelayedDestroy(spell.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
                if (spell.baseDamage > 0)
                {
                    DealDamage();
                }
            }
        
        }

        private void DealDamage()
        {
            // Debug.Log("DEALING DMG");
            var size = Physics2D.OverlapCircleNonAlloc(transform.position + spell.centerOffset, spell.damageRadius, _results, spell.enemyLayer);
            for (int i = 0; i < size; i++)
            {
                Debug.Log(_results[i].gameObject.name);
                try
                {
                    EnemyInterface enemy;
                    if (_results[i].isTrigger)
                    {
                        if (_results[i].CompareTag("Enemy Hitbox"))
                        {
                            // Debug.Log("Ennemy Hitbox!!");
                            enemy = _results[i].GetComponentInParent<EnemyInterface>();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (_results[i].CompareTag("Enemy"))
                        {
                            // Debug.Log("Ennemy!!");
                            enemy = _results[i].GetComponent<EnemyInterface>();
                        } else {
                            continue;
                        }
                    }

                    if (enemy == null)
                    {
                        // Debug.Log("Ennemy script null");
                        continue;
                    }
                    if (collidedEnnemiesId.Contains(enemy.id)){continue;}
                    
                    collidedEnnemiesId.Add(enemy.id);
                    StartCoroutine(DelayedRemoval(spell.interactionInterval>0 ? spell.interactionInterval : 5f, enemy.id));
                    
                    Debug.Log($"Dealing Damage to {enemy.gameObject.name}");
                    enemy.Damage(spell.Damage);
                    spell.ApplyStatus(enemy);
                    if (spell.hasOnHitEffect)
                    {
                        Instantiate(spell.onHitEffect, _results[i].transform.position, transform.rotation);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"PROBLEM WITH {_results[i]} Stacktrace: {e}");
                }
            }
        }



        IEnumerator DelayedDestroy(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    
        // If has trigger
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!spell.zoneSpell)
            {
                return;
            }
            
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

            if (enemyScript == null || collidedEnnemiesId.Contains(enemyScript.id))
            {
                return;
            }
            
            collidedEnnemiesId.Add(enemyScript.id);
            StartCoroutine(DelayedRemoval(spell.interactionInterval, enemyScript.id));
            
            enemyScript.TryDamage(spell.Damage, spell);
            spell.ApplyStatus(enemyScript);
            if (spell.hasOnHitEffect)
            {
                Instantiate(spell.onHitEffect, col.transform.position, transform.rotation);
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (!spell.zoneSpell)
            {
                return;
            }
            
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
                    enemyScript = col.GetComponentInParent<EnemyInterface>();
                }
                else
                {
                    return;
                }
            }

            if (enemyScript == null || collidedEnnemiesId.Contains(enemyScript.id))
            {
                return;
            }
            
            collidedEnnemiesId.Add(enemyScript.id);
            StartCoroutine(DelayedRemoval(spell.interactionInterval, enemyScript.id));
            
            enemyScript.TryDamage(spell.Damage, spell);
            spell.ApplyStatus(enemyScript);
            if (spell.hasOnHitEffect)
            {
                Instantiate(spell.onHitEffect, col.transform.position, transform.rotation);
            }
        }
        
        IEnumerator DelayedRemoval(float delay, int id)
        {
            yield return new WaitForSeconds(delay);
            collidedEnnemiesId.Remove(id);
        }


    }
}
