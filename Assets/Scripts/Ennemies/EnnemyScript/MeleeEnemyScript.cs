using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyScript : EnnemyInterface
{

    public CircleCollider2D playerDetection;
   
    // Start is called before the first frame update
    void Start()
    {
        _self = gameObject;
        Instance = this;
    }
    
    

    
    // Update is called once per frame

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isInRange = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
