using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellsList : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> spells = new List<GameObject>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {

        // Create space for 64 spells
        for (int i = 0; i < 63; i++) {
            spells.Add(null);
        }

        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Spells");
        foreach (GameObject obj in prefabs)
        {
            Debug.Log(obj.name);
            spells[obj.GetComponent<Spell>().id] = obj;
        }
    }

    public static GameObject getSpell(int id) {
        if (63 < id || id < 0) {return spells[0];}
        return spells[id];
    }

}
