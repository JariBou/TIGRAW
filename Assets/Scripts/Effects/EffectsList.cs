using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsList : MonoBehaviour
{
    public static List<GameObject> effects = new List<GameObject>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {

        // Create space for 64 spells
        for (int i = 0; i < 63; i++) {
            effects.Add(null);
        }

        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Effects");
        foreach (GameObject obj in prefabs) {
            effects[obj.GetComponent<ParticleScript>().id] = obj;
        }
    }

    public static GameObject geteffect(int id) {
        if (63 < id || id < 0) {return effects[0];}
        return effects[id];
    }
}
