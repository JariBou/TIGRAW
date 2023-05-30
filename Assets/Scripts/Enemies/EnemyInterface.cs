using System.Collections;
using System.Collections.Generic;
using PlayerBundle.BraceletUpgrade;
using Spells;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyInterface : MonoBehaviour
    {
        public float attack;

        public GameObject deathObj;
        public float baseSpeed = 2f;
        [HideInInspector]
        public float speed;

        public float MaxHealth;
        public float health;
        public bool isInRange;

        public float DmgInteractionDelay = 2f;
        public float DmgInteractionTimer = 0f;

        public int id;
        public Dictionary<StatusType, StatusEffect> StatusEffects = new();

        private bool shocked; // for now to know if ennemy has electric status effect

        public List<int> collidedSpellsId;
        private static readonly int IsDead = Animator.StringToHash("isDead");
        public SpriteRenderer renderer;
        public Animator animator;

        [FormerlySerializedAs("lootGameOBject")] [FormerlySerializedAs("loot")] [Header("Loot")] public GameObject lootGameObject;
        public int lootChance;
        public int lootNumber = 1;
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        public float Speed => speed / 100;
        public float localDifficulty = 1f;

        private GameManager _gm;


        public void Awake()
        {
            id = gameObject.GetInstanceID();
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            renderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            speed = baseSpeed;
        }

        private void Start()
        {
            MaxHealth *= localDifficulty;
            attack *= localDifficulty;
            health = MaxHealth;
        }

        public void Kill(bool ignoreLoot = false)
        {
            GameObject deadObj = Instantiate(deathObj, transform.position, Quaternion.identity);
            deadObj.transform.localScale = transform.localScale;

            if (!ignoreLoot)
            {
                CalculateLoot();
            }
                
            Destroy(gameObject);
        }


        public void Damage(float amount)
        {
            if (amount <= 0)
            {
                return;
            }

            if (shocked)
            {
                amount *= 1 + _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.StatikDmgIncrease);
            } 
            
            Debug.Log($"Enemy with id {id} took {amount}DMG || shocked={shocked}");
            StartCoroutine(FlashOnDmg());
            health -= amount;
            animator.SetTrigger(Hurt);
        }

        private IEnumerator FlashOnDmg()
        {
            renderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            renderer.color = Color.white;
        }

        protected void FixedUpdate()
        {
            if (health <= 0)
            {
                Kill();
                return;
            }
            
            ResolveStatusEffects();

            if (DmgInteractionTimer > 0)
            {
                DmgInteractionTimer -= Time.fixedDeltaTime;
            }

            if (DmgInteractionTimer < 0)
            {
                ResetInteractionTimer();
            }
        }

        private void ResolveStatusEffects()
        {
            foreach (var keyValuePair in StatusEffects)
            {
                StatusEffect statusEffect = keyValuePair.Value;
                StatusType type = keyValuePair.Key;

                if (statusEffect.isActive())
                {
                    statusEffect.Duration -= Time.fixedDeltaTime;
                    if (type == StatusType.Ice)
                    {
                        speed = (float)(baseSpeed * (0.9 - _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.IceSlowIncrease)));
                    } else if (type == StatusType.Electric)
                    {
                        shocked = true;
                    } else if (type == StatusType.Fire)
                    {
                        health -= statusEffect.Dps * Time.fixedDeltaTime * (1 +
                                                                            _gm.BraceletUpgradesHandler
                                                                                .GetUpgradedAmount(BraceletUpgrades
                                                                                    .FireEffectDmgIncrease)) ;
                    }
                }
                if (!statusEffect.isActive())
                {
                    if (type == StatusType.Ice)
                    {
                        speed = baseSpeed;
                    } else if (type == StatusType.Electric)
                    {
                        shocked = false;
                    }

                    statusEffect.Duration = 0;
                }
            }
        }

        private void CalculateLoot()
        {
            if (Random.Range(0, 100) <= lootChance)
            {
                for (int i = 0; i < lootNumber; i++)
                {
                    Instantiate(lootGameObject, transform.position, Quaternion.identity);
                }
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
                Damage(amount); // For now shocked enemies take twice the dmg
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
