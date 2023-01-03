using UnityEngine;

namespace MainMenusScripts
{
    public class PauseMenuScript : MonoBehaviour
    {
        public void ReturnToLobby()
        {
            //TODO: TimeScale stays at 0 when going from scene to scene
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
