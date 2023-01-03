using System;
using PlayerBundle;
using Spells.SpellBehavior;
using UnityEngine;
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
        public float damage = 10;
        public int id;
        public float heatProduction = 2f;

        public float interactionInterval;
    
        public LayerMask enemyLayer;
        public LayerMask wallLayer;

        internal Tilemap GroundTilemap;

    
        [HideInInspector]
        public GameObject startParticles, endParticles;

        public SpellsType spellType;

        [HideInInspector]
        public bool isInfPierce;

        [HideInInspector]
        public bool zoneSpell;
    
        [HideInInspector]
        public float spellDuration;
    
        [HideInInspector]
        public CircleCollider2D damageZone;

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

        // Start is called before the first frame update
        void Start()
        {
            GroundTilemap = Player.instance.groundTilemap;
            damageZone = GetComponent<CircleCollider2D>();
        
            MousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            MousePos += new Vector3(0, 0, 10); // z camera offset
            transform.position = Player.instance.GetPosition();
            Direction = (MousePos - Player.instance.GetPosition());
            Direction /= Direction.magnitude;

            switch (spellType)
            {
                case SpellsType.Dash :
                    Player.instance.isDashing = true;
                    gameObject.AddComponent<Dash>().spell = this;
                    break;
            
                case SpellsType.Teleport :
                    transform.position = MousePos;
                    gameObject.AddComponent<Teleport>().spell = this;
                    break;
            
                case SpellsType.Projectile :

                    RaycastHit2D hit = Physics2D.Raycast(Player.instance.GetPosition(), Direction,
                        new Vector2(Direction.x, Direction.y).sqrMagnitude, wallLayer);
        
                    Debug.DrawRay(new Vector3(Player.instance.GetPosition().x, Player.instance.GetPosition().y, 0), new Vector3(Direction.x, Direction.y, 0), Color.red, 1f);
                    if (hit)
                    {
                        Debug.LogError("Hit");
                        if (hit.transform.gameObject != null)
                        {
                            Debug.LogError("Has GameObject");
                            if (hit.transform.gameObject.CompareTag("Wall"))
                            {
                                Debug.LogError("Destroying");
                                Destroy(gameObject);
                            }
                        }
                    }
                
                    gameObject.AddComponent<Projectile>().spell = this;
                    break;

                case SpellsType.AoeCast :
                    transform.position = MousePos;
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
    
    }
}