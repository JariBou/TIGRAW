using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellsList : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> spells = new List<GameObject>();

    public bool LoadSpells = true;
    public string path = "Prefabs/Spells";
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {

        if (!LoadSpells)
        {
            return;
            
        }

        // Create space for 64 spells
        for (int i = 0; i < 63; i++) {
            spells.Add(null);
        }

        GameObject[] prefabs = Resources.LoadAll<GameObject>(path);
        foreach (GameObject obj in prefabs)
        {
            Debug.Log(obj.name);
            spells[obj.GetComponent<Spell>().id] = obj;
        }
    }

    public static GameObject getSpell(int id) {
        if (63 < id || id < 0) {return spells[0];}

        try
        {
            return spells[id];

        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading spell id: {id}");
            throw;
        }
    }

}
