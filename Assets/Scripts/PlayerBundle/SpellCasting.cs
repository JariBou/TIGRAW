using System;
using Spells;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace PlayerBundle
{
    public abstract class SpellCasting
    {

        // Work on Casting spells by type:
        public static void CastSpell(InputAction.CallbackContext context, int spellId)
        {
            Player player = Player.Instance;
            if (player.isTeleporting) return;
            if (!context.performed) return;
            if (player.gamePaused) return;
        
            GameObject spellPrefab;

            try
            {
                spellPrefab = SpellsList.GetSpell(spellId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            Debug.Log($"Casting Spell: {spellPrefab.name}");
            Spell spellPrefabScript = spellPrefab.GetComponent<Spell>();
            if (player.heatAmount + spellPrefabScript.heatProduction <= player.baseMaxHeat)
            {
                player.ResetTimer();
                Object.Instantiate(spellPrefab);
            }
        }

    
    }
}
