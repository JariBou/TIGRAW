using LoadingScripts;
using Saves;
using UnityEngine;

namespace MainMenusScripts
{
    public class PauseMenuScript : MonoBehaviour
    {
        public void ReturnToLobby()
        {
            Time.timeScale = 1;
            GameManager.LoadScene(3);
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
            GameManager.LoadScene(1);
        }
    }
}
