using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LobbyScripts
{
    public class TeleporterScript : Interactable
    {
        private bool _playerInRange;

        public int SceneToLoadId;

        public static TeleporterScript Instance;
        private GameManager _gm;
        

        private void Awake()
        {
            Instance = this;
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
            if (!isUsable) {return;}
            Debug.Log($"Interacting with {name}");
            //SceneManager.LoadScene(4);
            //SceneLoader.instance.LoadScene(4);
            _gm.LoadScene(SceneToLoadId);
            isUsable = false;
        }

        protected override void OnFlagEvent(Flag flag)
        {
            if (flag == Flag.UnlockTeleporter)
            {
                isUsable = true;
            }
        }
    }
}
