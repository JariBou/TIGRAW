using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spells
{
    public class SpellsList : MonoBehaviour
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static List<GameObject> _spells = new();

        [FormerlySerializedAs("LoadSpells")] public bool loadSpells = true;
        public string path = "Prefabs/Spells";
    
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {

            if (!loadSpells)
            {
                return;
            
            }

            // Create space for 64 spells
            for (int i = 0; i < 63; i++) {
                _spells.Add(null);
            }

            GameObject[] prefabs = Resources.LoadAll<GameObject>(path);
            foreach (GameObject obj in prefabs)
            {
                _spells[obj.GetComponent<Spell>().id] = obj;
            }
        }

        public static GameObject GetSpell(int id) {
            if (id is > 63 or < 0) {return _spells[0];}

            if (_spells[id] == null)
            {
                Debug.LogError($"Error loading spell id: {id}");
                throw new NotImplementedException();
            }
            return _spells[id];
        }

    }
}
