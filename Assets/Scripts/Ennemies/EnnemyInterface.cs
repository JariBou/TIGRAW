using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyInterface : MonoBehaviour
{
    public float attack;

    public float health;
    public GameObject _self;
    public bool isInRange;
    public static EnnemyInterface Instance;

    public int id;

    public void Awake()
    {
        id = gameObject.GetInstanceID();
    }


    public void Damage(float amount)
    {
        health -= amount;
    }

    
}
