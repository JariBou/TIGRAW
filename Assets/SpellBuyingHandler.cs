using System;
using System.Collections;
using System.Collections.Generic;
using Saves;
using ScriptableObjects;
using UnityEngine;

public class SpellBuyingHandler
{
    public List<SpellSO> unlockedSpells = new (64);

    public static event Action<SpellSO> SpellBought; // Might remove argument idk

    private GameManager _gm;


    public SpellBuyingHandler(GameManager gm)
    {
        _gm = gm;
    }
    
    public SpellBuyingHandler(GameManager gm, JsonSaveData data)
    {
        _gm = gm;
        foreach (SpellSO spellSo in gm.spellListSO)
        {
            if (data.unlockedSpellsSoName.Contains(spellSo.name))
            {
                unlockedSpells.Add(spellSo);
            }
        }
    }

    public void BuySpell(SpellSO spell)
    {
        unlockedSpells.Add(spell);
        OnSpellBought(spell);
    }

    private static void OnSpellBought(SpellSO spellSo)
    {
        SpellBought?.Invoke(spellSo);
    }

    public List<string> GetUnlockedSpellsName()
    {
        List<string> temp = new List<string>(unlockedSpells.Capacity);
        foreach (SpellSO spellSo in unlockedSpells)
        {
            temp.Add(spellSo.name); // save the name of the spellSo object
        }

        return temp;
    }
}
