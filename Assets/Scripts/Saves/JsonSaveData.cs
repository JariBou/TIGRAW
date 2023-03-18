using System;
using System.Collections.Generic;
using PlayerBundle.PlayerUpgrades;
using Saves.JsonDictionaryHelpers;
using Spells;
using UnityEngine;

namespace Saves
{
    [Serializable]
    public class JsonSaveData
    {
        // Save the object on Game Manager to not load from scene to scene but still save it to file

        public EnumIntDictionary playerUpgradesLvl = new();
        public EnumFloatDictionary upgradeAmount = new();

        public List<int> unlockedSpellsId;

        public int playerGold; // Might replace with souls to make more sense
        public int playerCrystals; 
        

        public static JsonSaveData Initialise()
        {
            JsonSaveData data = new JsonSaveData
            {
                playerUpgradesLvl =
                {
                    items = new EnumIntItem[PlayerUpgradesHandler.playerUpgradesLvl.Count]
                },
                upgradeAmount =
                {
                    items = new EnumFloatItem[PlayerUpgradesHandler.upgradeAmount.Count]
                },
                unlockedSpellsId = SpellsList.unlockedSpellsId
            };

            int i = 0;
            foreach (var keyValuePair in PlayerUpgradesHandler.playerUpgradesLvl)
            {
                data.playerUpgradesLvl.items[i] = new EnumIntItem { key = Enum.GetName(typeof(PlayerUpgrades), keyValuePair.Key), value = keyValuePair.Value };
                i++;
            }
            
            i = 0;
            foreach (var keyValuePair in PlayerUpgradesHandler.upgradeAmount)
            {
                data.upgradeAmount.items[i] = new EnumFloatItem { key = Enum.GetName(typeof(PlayerUpgrades), keyValuePair.Key), value = keyValuePair.Value};
                i++;
            }


            return data;
        }
        
    }
}