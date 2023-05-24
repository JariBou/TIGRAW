using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LootType
{
    Soul,
    Coin,
    Health
}


public class Loot : MonoBehaviour
{
    public static event Action<Loot> LootPickup ;

    public LootType type;

    public int value = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            OnLootPickup(this);
            Destroy(gameObject);
        }
    }

    private static void OnLootPickup(Loot obj)
    {
        LootPickup?.Invoke(obj);
    }
}
