using System;
using Spells;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace PlayerBundle
{
    public abstract class SpellCasting
    {
        //TODO: Make DAsh and TP spells, maybe an enum with the types and make this a super script that handles everything
        //TODO: so that it makes the player script lighter
    
        //TODO: Add checkers for if spells can be casted on spell scripts to make a simpler player movement script
    
        // Work on Casting spells by type:
        public static void CastSpell(InputAction.CallbackContext context, int spellId)
        {
            Player player = Player.instance;
            if (player.isTeleporting) return;
            if (!context.performed) return;
            if (player.gamePaused) return;
        
            player.ResetTimer();
            GameObject spellPrefab;

            try
            {
                spellPrefab = SpellsList.getSpell(spellId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        
        
            Debug.Log($"Casting Spell: {spellPrefab.name}");
            Spell spellPrefabScript = spellPrefab.GetComponent<Spell>();
            if (player.heatAmount + spellPrefabScript.heatProduction <= 100)
            {
                Object.Instantiate(spellPrefab);
            }
        }

    
    }
}
