using UnityEngine;

namespace LobbyScripts
{
    public class ChaliceScript : Interactable
    {
        public Canvas canvas;

        private GameManager _gm;
        // Start is called before the first frame update
        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        void Start()
        {
            canvas.enabled = false;
        }
        
        public override void Interact()
        {
            if (canvas.enabled)
            {
                canvas.enabled = false;
                _gm.isInMenu = false;
                return;
            }

            _gm.isInMenu = true;
            canvas.enabled = true;
        }

        protected override void OnFlagEvent(Flag flag)
        {
            
        }
    }
}
