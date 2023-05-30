using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class SpellList : ScriptableObject, IEnumerable
    {
        public List<SpellSO> listOfSpells;
        // Imo Id of spell will be the index in that list
        public SpellSO this[int i] => listOfSpells[i];

        public int Size => listOfSpells.Count;
        public IEnumerator GetEnumerator()
        {
            return listOfSpells.GetEnumerator();
        }

        public bool Contains(SpellSO spellSo)
        {
            return listOfSpells.Contains(spellSo);
        }

        public SpellSO Find(string spellSoName, SpellSO defaultValue)
        {
            int index = listOfSpells.FindIndex(so => so.name == spellSoName);
            if (index == -1)
            {
                return defaultValue;
            }

            return listOfSpells[index];
        }
    }
}