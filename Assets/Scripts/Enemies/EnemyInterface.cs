using System.Collections;
using System.Collections.Generic;
using Spells;
using UnityEngine;

namespace Enemies
{
    public class EnemyInterface : MonoBehaviour
    {
        public float attack;

        public GameObject deathObj;

        public float MaxHealth;
        public float health;
        public GameObject self;
        public bool isInRange;
        public static EnemyInterface Instance;

        public float DmgInteractionDelay = 2f;
        public float DmgInteractionTimer = 0f;

        public int id;
        
        public List<int> collidedSpellsId;
        private static readonly int IsDead = Animator.StringToHash("isDead");


        public void Awake()
        {
            id = gameObject.GetInstanceID();
            health = MaxHealth;
        }


        public void Damage(float amount)
        {
            health -= amount;
        }

        protected void FixedUpdate()
        {
            if (health <= 0)
            {
                Instantiate(deathObj, transform.position, Quaternion.identity);
                Destroy(gameObject);
                return;
            }
            
            if (DmgInteractionTimer > 0)
            {
                DmgInteractionTimer -= Time.fixedDeltaTime;
            }

            if (DmgInteractionTimer < 0)
            {
                ResetInteractionTimer();
            }
        }

        public void InitInteractionTimer()
        {
            DmgInteractionTimer = DmgInteractionDelay;
        }
        
        public void ResetInteractionTimer()
        {
            DmgInteractionTimer = 0f;
        }


        public void TryDamage(float amount, Spell spell)
        {
            if (collidedSpellsId.Contains(spell.Id))
            {
                //Debug.Log($"CollidedSpellsId already contains {spell.id}");
            }
            else
            {
                Damage(amount);
                collidedSpellsId.Add(spell.Id);
                StartCoroutine(DelayedRemoval(spell.interactionInterval, spell.Id));
            }
        }
        
        IEnumerator DelayedRemoval(float delay, int id)
        {
            yield return new WaitForSeconds(delay);
            collidedSpellsId.Remove(id);
        }
        
        
    }
}
