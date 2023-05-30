using System;
using Enemies;
using UnityEngine;

namespace Spells.SpellBehavior
{
    public class Teleport : MonoBehaviour
    {
    
        public Spell spell;
        
        private Collider2D[] _results = new Collider2D[32];


        // Start is called before the first frame update
        void Start()
        {
            if (!spell.GroundTilemap.HasTile(spell.GroundTilemap.WorldToCell(spell.MousePos)))
            {
                Debug.Log("DESTROYING");
                Destroy(gameObject);
                return;
            }
        
            spell.player.isTeleporting = true;
            spell.player.spriteRenderer.enabled = false;
            spell.player.SetVelocity(Vector2.zero);

            spell.player.heatManager.AddHeat(spell.heatProduction);

            GameObject thingy = Instantiate(spell.startParticles, spell.player.transform.position, Quaternion.identity);
            //Destroy(thingy.gameObject, 0.5f);
            if (spell.Damage > 0)
            {
                DealDamage();
            }

            Invoke("DoTeleport", spell.projectileSpeed);

        }
        
        private void DealDamage()
        {
            Debug.Log("============== DEALING DAMAGE ==============");
            var size = Physics2D.OverlapCircleNonAlloc(transform.position + spell.centerOffset, spell.damageRadius, _results, spell.enemyLayer);
            Debug.Log($"Size={size}");
            
            for (int i = 0; i < size; i++)
            {
                try
                {
                    if (!_results[i].CompareTag("Enemy")) continue; // If enemy additional hitbox basically
                    
                    EnemyInterface enemy = _results[i].GetComponent<EnemyInterface>();
                    enemy.Damage(spell.Damage);
                    spell.ApplyStatus(enemy);
                }
                catch (Exception e)
                {
                    Debug.LogError($"PROBLEM WITH {_results[i]} Stacktrace: {e}");
                }
            }
        }


        void DoTeleport()
        {
            Debug.Log("TELEPORTING");
            transform.position = spell.MousePos;
            
            var position = transform.position;
            spell.player.transform.position = position;
            
            GameObject thingy = Instantiate(spell.endParticles, position, Quaternion.identity);
            //Destroy(thingy.gameObject, 0.5f);

            spell.player.isTeleporting = false;
            spell.player.spriteRenderer.enabled = true;
            if (spell.Damage > 0)
            {
                DealDamage();
            }
            Destroy(gameObject);
        }
    }
}
