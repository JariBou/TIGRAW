using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Enemies.EnemyScript;
using PlayerBundle;
using UnityEngine;

// Cast will be used for AOE Attacks around player AND AOE Targeted Attacks
namespace Spells.SpellBehavior
{
    public class Cast : MonoBehaviour
    {
    
        public Spell spell;
        public List<int> collidedEnemiesId;


    
        // Start is called before the first frame update
        void Start()
        {
            Player.instance.heatAmount += spell.heatProduction;
            collidedEnemiesId = new List<int>();


            if (spell.zoneSpell)
            {
                StartCoroutine(DelayedDestroy(spell.spellDuration));
                InvokeRepeating("DealDamage", 0, spell.interactionInterval); //TODO: doesn't seem right
            }
            else
            {
                StartCoroutine(DelayedDestroy(spell.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
                if (spell.damage > 0)
                {
                    DealDamage();
                }
            }
        
        }

        void DealDamage()
        {
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, spell.DamageRadius, spell.enemyLayer);

            for (int i = 0; i < results.Length; i++)
            {
                try
                {
                    results[i].GetComponent<MeleeEnemyScript>().Damage(spell.damage);

                }
                catch (Exception e)
                {
                    Debug.LogError($"PROBLEM WITH {results[i]} Stacktrace: {e}");
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
            if (!col.CompareTag("Enemy")) return;
            
            EnemyInterface enemyScript = col.GetComponent<EnemyInterface>();
            if (collidedEnemiesId.Contains(enemyScript.id))
            {
                return;
            }

            collidedEnemiesId.Add(enemyScript.id);
            StartCoroutine(DelayedRemoval(spell.interactionInterval, enemyScript.id));
            enemyScript.Damage(spell.damage);
        }
    
        IEnumerator DelayedRemoval(float delay, int id)
        {
            yield return new WaitForSeconds(delay);
            collidedEnemiesId.Remove(id);
        }

    }
}
