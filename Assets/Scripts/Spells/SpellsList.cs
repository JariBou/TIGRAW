using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
    public class SpellsList : MonoBehaviour
    {
        private static List<GameObject> spells = new List<GameObject>();

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
                spells[obj.GetComponent<Spell>().id] = obj;
            }
        }

        public static GameObject getSpell(int id) {
            if (id is > 63 or < 0) {return spells[0];}

            if (spells[id] == null)
            {
                Debug.LogError($"Error loading spell id: {id}");
                throw new NotImplementedException();
            }
            return spells[id];
        }

    }
}
