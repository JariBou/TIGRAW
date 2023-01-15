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

        public void ReturnToMenu()
        {
            Time.timeScale = 1;
            GameManager.LoadScene(1);
        }
    }
}
