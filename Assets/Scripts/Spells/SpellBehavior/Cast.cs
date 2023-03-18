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
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Collider2D[] _results = new Collider2D[32];


    
        // Start is called before the first frame update
        void Start()
        {
            Player.Instance.heatAmount += spell.heatProduction;


            if (spell.zoneSpell)
            {

                CircleCollider2D collider2D = gameObject.AddComponent<CircleCollider2D>();
                collider2D.radius = spell.damageRadius;
                collider2D.isTrigger = true;
                
                
                StartCoroutine(DelayedDestroy(spell.spellDuration));
                InvokeRepeating("DealDamage", 0, spell.interactionInterval); //TODO: doesn't seem right
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
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, spell.damageRadius, _results, spell.enemyLayer);

            for (int i = 0; i < size; i++)
            {
                try
                {
                    _results[i].GetComponent<EnemyInterface>().Damage(spell.Damage);

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
            
            if (!col.CompareTag("Enemy")) return;
            
            EnemyInterface enemyScript = col.GetComponent<EnemyInterface>();
            
            enemyScript.TryDamage(spell.Damage, spell);
            
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (!spell.zoneSpell)
            {
                return;
            }
            
            if (!col.CompareTag("Enemy")) return;
            
            EnemyInterface enemyScript = col.GetComponent<EnemyInterface>();
            
            enemyScript.TryDamage(spell.Damage, spell);
        }


    }
}
