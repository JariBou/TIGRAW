using PlayerBundle.PlayerUpgrades;
using UnityEngine;

namespace LobbyScripts
{
    public class ShopStatueScript : Interactable
    {
        [SerializeField]
        private SpellShopManager manager;

        public override void Interact()
        {
            if (manager.isReassigning)
            {
                manager.gm.isInMenu = false;    // Part 1 of fixing using ESC to cancel spell assignment
                manager.StopReassignSpell();
            } else
            {
                manager.Toggle();
            }
        }

        protected override void OnFlagEvent(Flag flag)
        {
            
        }
    }
}
