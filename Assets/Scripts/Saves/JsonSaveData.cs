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

        // relic from when I worked on this while filming an AD for MEUPORG

        public static JsonSaveData Initialise()
        {
            GameManager gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            JsonSaveData data = new JsonSaveData
            {
                playerUpgradesLvl =
                {
                    items = new EnumIntItem[gm.PlayerUpgradesHandler.UpgradesLvl.Count]
                },
                upgradeAmount =
                {
                    items = new EnumFloatItem[gm.PlayerUpgradesHandler.UpgradesAmount.Count]
                },
                unlockedSpellsId = SpellsList.unlockedSpellsId
            };

            int i = 0;
            foreach (var keyValuePair in gm.PlayerUpgradesHandler.UpgradesLvl)
            {
                data.playerUpgradesLvl.items[i] = new EnumIntItem { key = Enum.GetName(typeof(PlayerUpgrades), keyValuePair.Key), value = keyValuePair.Value };
                i++;
            }
            
            i = 0;
            foreach (var keyValuePair in gm.PlayerUpgradesHandler.UpgradesAmount)
            {
                data.upgradeAmount.items[i] = new EnumFloatItem { key = Enum.GetName(typeof(PlayerUpgrades), keyValuePair.Key), value = keyValuePair.Value};
                i++;
            }


            return data;
        }
        
    }
}