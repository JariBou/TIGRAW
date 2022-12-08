using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCasting
{
    //TODO: Make DAsh and TP spells, maybe an enum with the types and make this a super script that handles everything
    //TODO: so that it makes the player script lighter
    
    // Work on Casting spells by type:
    public static void CastSpell(InputAction.CallbackContext context, int spellId)
    {
        Player player = Player.instance;
        if (player.isTeleporting) return;
        if (!context.performed) return;
        
        player.ResetTimer();
        
        GameObject spellPrefab = SpellsList.getSpell(spellId);
        Debug.Log(spellPrefab.name);
        Spell spellPrefabScript = spellPrefab.GetComponent<Spell>();
        if (player.heatAmount + spellPrefabScript.heatProduction <= 100)
        {
            Object.Instantiate(spellPrefab);
        }
    }

    
}
