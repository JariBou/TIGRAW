using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    
    public Spell spell;
    
    
    // Start is called before the first frame update
    void Start()
    {

        if (!Player.instance.floor.bounds.Contains(transform.position))
        {
            Debug.Log("DESTROYING");
            Destroy(gameObject);
            return;
        }
        Player.instance.isTeleporting = true;
        Player.instance.sprite.enabled = false;
        Player.instance.SetVelocity(Vector2.zero);

        Player.instance.heatAmount += spell.heatProduction;

        Instantiate(spell.startParticles, Player.instance.transform.position, Quaternion.identity);
        Invoke("DoTeleport", spell.projectileSpeed);

    }

    void DoTeleport()
    {
        Debug.Log("TELEPORTING");
        Debug.Log(Player.instance.transform.position);
        Debug.Log(transform.position);
        Player.instance.transform.position = transform.position;
        Instantiate(spell.endParticles, transform.position, Quaternion.identity);
        Player.instance.isTeleporting = false;
        Player.instance.sprite.enabled = true;

        Destroy(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
