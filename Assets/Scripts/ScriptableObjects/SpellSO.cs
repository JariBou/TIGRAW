using System;
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
        
        public int spellCost;
        public String spellName;
        public String spellDescription;
        
        public bool isUnlocked; // Idk
        [FormerlySerializedAs("spellId")] public int spellTypeId; // -1 == null, 0 == regularspell

    }
}