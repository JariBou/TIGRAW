using PlayerBundle.PlayerUpgrades;
using UnityEngine;

namespace LobbyScripts
{
    public class LevelUpStatueScript : Interactable
    {
        public Canvas canvas;
        // Start is called before the first frame update
        void Start()
        {
            canvas.enabled = false;
        }
        
        public override void Interact()
        {
            if (canvas.enabled)
            {
                canvas.enabled = false;
                return;
            }
            canvas.enabled = true;
            PlayerUpgrade[] upgradesScripts = canvas.GetComponentsInChildren<PlayerUpgrade>();
            foreach (PlayerUpgrade script in upgradesScripts)
            {
                script.Refresh();
            }
        }

        public override void OnFlagEvent(Flag flag)
        {
            
        }
    }
}
