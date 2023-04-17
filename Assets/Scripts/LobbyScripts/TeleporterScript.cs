using System;
using UnityEngine;

namespace LobbyScripts
{
    public class TeleporterScript : Interactable
    {
        private bool _playerInRange;

        public int SceneToLoadId;
        
        public static TeleporterScript Instance;

        private void Awake()
        {
            Instance = this;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
            }
        }

        public override void Interact()
        {
            if (!IsUsable) {return;}
            Debug.Log($"Interacting with {name}");
            //SceneManager.LoadScene(4);
            //SceneLoader.instance.LoadScene(4);
            GameManager.LoadScene(SceneToLoadId);
        }

        protected override void OnFlagEvent(Flag flag)
        {
            if (flag == Flag.UnlockTeleporter)
            {
                IsUsable = true;
            }
        }
    }
}
