using System;
using System.Collections.Generic;
using PlayerBundle.BraceletUpgrade;
using PlayerBundle.PlayerUpgrades;
using Saves.JsonDictionaryHelpers;
using Spells;
using UnityEngine;
using UnityEngine.Serialization;

namespace Saves
{
    [Serializable]
    public class JsonSaveData
    {
        // Save the object on Game Manager to not load from scene to scene but still save it to file
        public string saveName = "Save 1";
        
        public EnumIntDictionary braceletUpgradesLvl = new();
        public EnumFloatDictionary upgradeAmount = new();
        public StringStringDictionary bindedSpells = new();

        public List<string> unlockedSpellsSoName = new();

        public int playerGold; // Might replace with souls to make more sense
        public int playerSouls;

        // relic from when I worked on this while filming an AD for MEUPORG

        public static JsonSaveData Initialise()
        {
            GameManager gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            JsonSaveData data = new JsonSaveData
            {
                braceletUpgradesLvl =
                {
                    items = new EnumIntItem[gm.BraceletUpgradesHandler.UpgradesLvl.Count]
                },
                upgradeAmount =
                {
                    items = new EnumFloatItem[gm.BraceletUpgradesHandler.UpgradesAmount.Count]
                },
                bindedSpells =
                {
                    items = new StringStringItem[gm.spellBindingsSo.Count]
                },
                playerSouls = gm.currentSave.playerSouls,
                unlockedSpellsSoName = gm.SpellBuyingHandler.GetUnlockedSpellsName(),
                saveName = "Save 1"
            };

            int i = 0;
            foreach (var keyValuePair in gm.BraceletUpgradesHandler.UpgradesLvl)
            {
                data.braceletUpgradesLvl.items[i] = new EnumIntItem { key = Enum.GetName(typeof(BraceletUpgrades), keyValuePair.Key), value = keyValuePair.Value };
                i++;
            }
            
            i = 0;
            foreach (var keyValuePair in gm.BraceletUpgradesHandler.UpgradesAmount)
            {
                data.upgradeAmount.items[i] = new EnumFloatItem { key = Enum.GetName(typeof(BraceletUpgrades), keyValuePair.Key), value = keyValuePair.Value};
                i++;
            }
            
            i = 0;
            foreach (var keyValuePair in gm.spellBindingsSo)
            {
                data.bindedSpells.items[i] = new StringStringItem { key = keyValuePair.Key, value = keyValuePair.Value.name};
                i++;
            }
            
            
            return data;
        }
    }
}