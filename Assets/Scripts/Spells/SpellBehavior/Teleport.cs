using System;
using Enemies;
using PlayerBundle;
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
        
            Player.Instance.isTeleporting = true;
            Player.Instance.sprite.enabled = false;
            Player.Instance.SetVelocity(Vector2.zero);

            Player.Instance.heatAmount += spell.heatProduction;
            
            GameObject thingy = Instantiate(spell.startParticles, Player.Instance.transform.position, Quaternion.identity);
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
                    _results[i].GetComponent<EnemyInterface>().Damage(spell.Damage);
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
            Player.Instance.transform.position = position;
            
            GameObject thingy = Instantiate(spell.endParticles, position, Quaternion.identity);
            //Destroy(thingy.gameObject, 0.5f);

            Player.Instance.isTeleporting = false;
            Player.Instance.sprite.enabled = true;
            if (spell.Damage > 0)
            {
                DealDamage();
            }
            Destroy(gameObject);
        }
    }
}
