using PlayerBundle;
using Saves;
using UnityEngine;

namespace MainMenusScripts
{

    public class PauseMenuScript : MonoBehaviour
    {
        
        private GameManager _gm;
        private Player player;
        private Canvas canvas;
        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            canvas = GetComponent<Canvas>();
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        public void ReturnToLobby()
        {
            Time.timeScale = 1;
            canvas.enabled = false;
            _gm.LoadScene(SceneBuildIndex.Lobby);
        }

        public void Resume()
        {
            Time.timeScale = player.gamePaused ? 1 : 0;

            player.gamePaused = !player.gamePaused;
            player.pauseMenuCanvas.enabled = player.gamePaused;
        }

        public void GoToSettings()
        {
            
        }
        [ContextMenu("SaveData")]
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
