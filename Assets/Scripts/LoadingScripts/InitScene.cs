using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadingScripts
{
    public class InitScene : MonoBehaviour
    {
        private void Start()
        {
            int screenWidth = SimpleSaveManager.GetInt("screenWidth", -1);
            int screenHeight = SimpleSaveManager.GetInt("screenHeight", -1);
            bool isFullscreen = SimpleSaveManager.GetBool("Fullscreen");
            int screenRefreshRate = SimpleSaveManager.GetInt("screenRefreshRate", 60);
        
            if (!(screenWidth == -1 || screenHeight == -1))
            {
                Screen.SetResolution(screenWidth, screenHeight, isFullscreen, screenRefreshRate);
            }

            Invoke("StartGame", 2f);
        }


        void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    
    }
}
