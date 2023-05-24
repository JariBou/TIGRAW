using LoadingScripts;
using Saves;
using UnityEngine;

namespace MainMenusScripts
{

    public class PauseMenuScript : MonoBehaviour
    {
        
        private GameManager _gm;
        private Canvas canvas;
        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            canvas = GetComponent<Canvas>();
        }

        public void ReturnToLobby()
        {
            Time.timeScale = 1;
            canvas.enabled = false;
            _gm.LoadScene(SceneBuildIndex.Lobby);
        }

        public void GoToSettings()
        {
            
        }

        public void Save()
        {
            SaveManager.SaveToJson(JsonSaveData.Initialise());
        }

        public void ReturnToMenu()
        {
            Time.timeScale = 1;
            canvas.enabled = false;
            _gm.LoadScene(SceneBuildIndex.MainMenu);
        }
    }
}
