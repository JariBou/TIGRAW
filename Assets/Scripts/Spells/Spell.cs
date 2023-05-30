using System;
using Enemies;
using PlayerBundle;
using PlayerBundle.BraceletUpgrade;
using Spells.SpellBehavior;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Spells
{
    public enum SpellsType
    {
        Dash,
        Teleport,
        Projectile,
        AoeCast,
        AoePlayer
    }


    [Serializable]
    public class Spell : MonoBehaviour
    {
        public float projectileSpeed = 2f;
        [FormerlySerializedAs("damage")] public float baseDamage = 10;
        [Serialize] public float Damage => (baseDamage + _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.AttackIncrease)) * player.AtkMultiplier;
        public int id; // Useless except for Spell List
        public float heatProduction = 2f;

        public float interactionInterval;
    
        public LayerMask enemyLayer;
        public LayerMask wallLayer;

        internal Tilemap GroundTilemap;

        [SerializeField]
        public StatusEffect StatusEffect = new StatusEffect(StatusType.Null, 0, 0);

    
        [HideInInspector]
        public GameObject startParticles, endParticles;
        public SpellsType spellType;
        [HideInInspector]
        public bool destroyOnAnimEnd;
        [HideInInspector]
        public bool isInfPierce;
        [HideInInspector]
        public bool zoneSpell;
        [HideInInspector]
        public float spellDuration;
        [HideInInspector]
        public Vector3 centerOffset;
        [HideInInspector]
        public CircleCollider2D damageZone;
        [HideInInspector]
        public bool hasOnHitEffect;
        [HideInInspector]
        public GameObject onHitEffect;

        // Interesting... ty Vampire Survivors lmao
        [HideInInspector]
        public int pierce = 1;
    
        // If it passes through walls(terrain) or not
        [HideInInspector]
        public bool phantom;
    
        //TEst for dynamic dis^play of radius
        [HideInInspector] public float damageRadius;

        [HideInInspector] public int dashDistance;
    

        // Represents the direction Vector from player to mouse
        [NonSerialized]
        public Vector2 Direction = Vector2.zero;

        internal Vector3 MousePos;

        public Player player;
        private GameManager _gm;

        [HideInInspector] public int Id;

        private void Awake()
        {
            Id = gameObject.GetInstanceID();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // What's the difference with FindWithTag
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); 
        }

        // Start is called before the first frame update
        void Start()
        {
            GroundTilemap = player.groundTilemap;
            damageZone = GetComponent<CircleCollider2D>();
        
            MousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            MousePos += new Vector3(0, 0, 10); // z camera offset
            transform.position = player.GetPosition();
            Direction = (MousePos - player.GetPosition());
            Direction /= Direction.magnitude;
            
            

            switch (spellType)
            {
                case SpellsType.Dash :
                    player.isDashing = true;
                    gameObject.AddComponent<Dash>().spell = this;
                    break;
            
                case SpellsType.Teleport :
                    gameObject.AddComponent<Teleport>().spell = this;
                    break;
            
                case SpellsType.Projectile :

                    if (!phantom)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(player.GetPosition(), Direction,
                            new Vector2(Direction.x, Direction.y).sqrMagnitude, wallLayer);

                        Debug.DrawRay(new Vector3(player.GetPosition().x, player.GetPosition().y, 0),
                            new Vector3(Direction.x, Direction.y, 0), Color.red, 1f);
                        if (hit)
                        {
                            if (hit.transform.gameObject != null)
                            {
                                if (hit.transform.gameObject.CompareTag("Wall"))
                                {
                                    Destroy(gameObject);
                                }
                            }
                        }
                    }

                    if (Direction.x < 0)
                    {
                        GetComponent<SpriteRenderer>().flipY = true;
                        GetComponent<CircleCollider2D>().offset *= new Vector2(1, -1);
                    }

                    float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                    gameObject.AddComponent<Projectile>().spell = this;
                    break;

                case SpellsType.AoeCast :
                    transform.position = MousePos - centerOffset;
                    gameObject.AddComponent<Cast>().spell = this;
                    break;
            
                case SpellsType.AoePlayer :
                    gameObject.AddComponent<Cast>().spell = this;
                    break;
            }
        }

        public void SetDirection(Vector2 direction) {
            Direction = direction;
        }

        public void SetPosition(Vector2 position) {
            transform.position = position;
        }

        public float GetHeat() {
            return heatProduction;
        }

        public void ApplyStatus(EnemyInterface enemy)
        {
            if (enemy == null) {return;}
            if (StatusEffect.StatusType == StatusType.Null)
            {
                return;
            }

            if (enemy.StatusEffects.ContainsKey(StatusEffect.StatusType))
            {
                StatusEffect stat = enemy.StatusEffects[StatusEffect.StatusType];
                enemy.StatusEffects[StatusEffect.StatusType] = StatusEffect.BestOf(stat, StatusEffect.Copy());
            }
            else
            {
                enemy.StatusEffects[StatusEffect.StatusType] = StatusEffect.Copy();
            }
            
        }
    
    }
}