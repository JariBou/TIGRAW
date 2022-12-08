using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Spell spell;

    private bool _hasCollided = false;
    
    // Start is called before the first frame update
    void Start()
    {

        RaycastHit2D hit = Physics2D.Raycast(Player.instance.GetPosition(), spell.direction,
            new Vector2(spell.direction.x, spell.direction.y).sqrMagnitude, spell.wallLayer);
        

        if (hit.transform.gameObject != null)
        {
            if (hit.transform.gameObject.CompareTag("Wall"))
            {

                Destroy(this);
            }
        }
        Player.instance.heatAmount += spell.heatProduction;

        
        
        gameObject.transform.position += new Vector3(spell.direction.x, spell.direction.y, 0); // Offset so it casts a bit in front of you
        // That way you can cast while next to a wall lol
        //TODO: You can cast through a small enough wall, fix it 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 transformVector = transform.position;
        transform.position = new Vector2(transformVector.x, transformVector.y) + spell.direction * (spell.projectileSpeed * Time.fixedDeltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        
        if (collider.CompareTag("Enemy") )
        {
            Debug.Log("HIT ENNEMY!");
            collider.GetComponent<MeleeEnemyScript>().Damage(spell.damage);
            if (!spell.isInfPierce)
            {
                Debug.Log("Somehow it's not infinite dude");
                _hasCollided = true;
            }
        } else if (collider.CompareTag("Wall") )
        {
            Debug.Log("Hits wall!!!");
            if (!spell.phantom)
            {
                Debug.Log("No phantom fucker");
                _hasCollided = true;
            }
        }
        
        if (_hasCollided)
        {
            Destroy(gameObject, 0.1f); // Actually only destroy if hasCollided
        }
        
    }
    
}
