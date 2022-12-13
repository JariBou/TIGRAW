using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    internal Tilemap groundTilemap;

    
    [HideInInspector]
    public GameObject startParticles, endParticles;

    public SpellsType spellType;

    [NonSerialized] public bool isProjectile, isAOE; 
    
    [HideInInspector]
    public bool isInfPierce;
    
    [HideInInspector]
    public CircleCollider2D damageZone;

    // Interesting... ty Vampire Survivors lmao
    [HideInInspector]
    public int pierce = 1;
    
    // If it passes through walls(terrain) or not
    [HideInInspector]
    public bool phantom;
    
    //TEst for dynamic dis^play of radius
    [HideInInspector] public float DamageRadius;

    [HideInInspector] public int dashDistance;
    

    // Represents the direction Vector from player to mouse
    [NonSerialized]
    public Vector2 direction = Vector2.zero;

    internal Vector3 mousePos;

    private bool hasCollided;
    // Start is called before the first frame update
    void Start()
    {
        groundTilemap = Player.instance.groundTilemap;
        damageZone = GetComponent<CircleCollider2D>();
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos += new Vector3(0, 0, 10); // z camera offset
        transform.position = Player.instance.GetPosition();
        direction = (mousePos - Player.instance.GetPosition());
        direction /= direction.magnitude;

        switch (spellType)
        {
            case SpellsType.Dash :
                Player.instance.isDashing = true;
                gameObject.AddComponent<Dash>().spell = this;
                break;
            
            case SpellsType.Teleport :
                transform.position = mousePos;
                gameObject.AddComponent<Teleport>().spell = this;
                break;
            
            case SpellsType.Projectile :
                isProjectile = true;
                
                RaycastHit2D hit = Physics2D.Raycast(Player.instance.GetPosition(), direction,
                    new Vector2(direction.x, direction.y).sqrMagnitude, wallLayer);
        
                Debug.DrawRay(new Vector3(Player.instance.GetPosition().x, Player.instance.GetPosition().y, 0), new Vector3(direction.x, direction.y, 0), Color.red, 1f);
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
                isAOE = true;
                transform.position = mousePos;
                gameObject.AddComponent<Cast>().spell = this;
                break;
            
            case SpellsType.AoePlayer : 
                isAOE = true;
                gameObject.AddComponent<Cast>().spell = this;
                break;
        };

    }

    public void SetDirection(Vector2 direction) {
        this.direction = direction;
    }

    public void SetPosition(Vector2 position) {
        transform.position = position;
    }

    public float getHeat() {
        return heatProduction;
    }
    
}
