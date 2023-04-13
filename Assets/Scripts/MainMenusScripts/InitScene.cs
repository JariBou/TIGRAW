using System;
using System.Collections.Generic;
using MainMenusScripts;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace LoadingScripts
{
    public class InitScene : MonoBehaviour
    {
        public AudioMixer audioMixer;

        private void Start()
        {
            
            print(typeof(Dictionary<dynamic, dynamic>).Name );
            print(typeof(Dictionary<int, string>).Name);
            print(typeof(Dictionary<int, string>).Name == typeof(Dictionary<dynamic, dynamic>).Name);
            
            
            // SaveManager.SaveData(new Vector2());
            // SaveManager.SaveData(new Vector3());
            // SaveManager.SaveData(5);
            // SaveManager.SaveData("test");
            // SaveManager.SaveData(new Dictionary<dynamic, int>());
            // SaveManager.SaveData(new Dictionary<int, string>());
            // SaveManager.SaveData(new Dictionary<string, dynamic>());
            
            
            
            
            int screenWidth = SimpleSaveManager.GetInt("screenWidth", -1);
            int screenHeight = SimpleSaveManager.GetInt("screenHeight", -1);
            bool isFullscreen = SimpleSaveManager.GetBool("Fullscreen");
            int screenRefreshRate = SimpleSaveManager.GetInt("screenRefreshRate", 60);
        
            if (!(screenWidth == -1 || screenHeight == -1))
            {
                Screen.SetResolution(screenWidth, screenHeight, isFullscreen, screenRefreshRate);
            }
            InitAudioVolume(audioMixer);


            StartGame();
        }


        void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        private float GetVolumeSliderValue()
        {
            float audioValue = SimpleSaveManager.GetFloat("Volume", 0f);
            Debug.LogWarning($"audioValue: {audioValue}");
            return Mathf.Pow(10, audioValue / 20);
        }

        private void SetVolume(float volume, AudioMixer audioMixer)
        {
            float value = Mathf.Log10(volume) * 20; // Volume mixer is not linear
            audioMixer.SetFloat("MasterVolume", value); 
            SimpleSaveManager.SaveFloat("Volume", value);
        }

        private void InitAudioVolume(AudioMixer audioMixer)
        {
            SetVolume(GetVolumeSliderValue(), audioMixer);
        }
    
    }
}