using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadingScripts
{
    public class InitScene : MonoBehaviour
    {
        private void Start()
        {
            
            print(typeof(Dictionary<dynamic, dynamic>).Name );
            print(typeof(Dictionary<int, string>).Name);
            print(typeof(Dictionary<int, string>).Name == typeof(Dictionary<dynamic, dynamic>).Name);
            
            
            SaveManager.SaveData(new Vector2());
            SaveManager.SaveData(new Vector3());
            SaveManager.SaveData(5);
            SaveManager.SaveData("test");
            SaveManager.SaveData(new Dictionary<dynamic, int>());
            SaveManager.SaveData(new Dictionary<int, string>());
            SaveManager.SaveData(new Dictionary<string, dynamic>());
            
            
            
            
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
