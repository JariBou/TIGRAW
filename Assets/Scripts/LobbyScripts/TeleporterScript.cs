using UnityEngine;

namespace LobbyScripts
{
    public class TeleporterScript : Interactable
    {
        private bool playerInRange;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (playerInRange)
            {
                // Show Tooltip of key to press
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }

        public override void Interact()
        {
            Debug.Log($"Interacting with {name}");

            //SceneManager.LoadScene(4);
            //SceneLoader.instance.LoadScene(4);
            GameManager.LoadScene(4);
        }
    }
}
