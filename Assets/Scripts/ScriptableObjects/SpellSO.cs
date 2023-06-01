using System;
using Spells;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class SpellSO : ScriptableObject
    {
        public GameObject spellPrefab;
        public Sprite icon;

        public SpellsType SpellsType = SpellsType.Null;
        public int spellCost;
        public int spellOverdriveCost;
        public String spellName;
        public String spellDescription;

        [FormerlySerializedAs("spellId")] public int spellTypeId; // -1 == null, 0 == regularspell

        private void OnValidate()
        {
            if (!spellPrefab) {return;}
            SpellsType = spellPrefab.GetComponent<Spell>().spellType;
            spellOverdriveCost = (int)spellPrefab.GetComponent<Spell>().heatProduction;
        }
    }
}