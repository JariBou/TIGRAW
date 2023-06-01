using System;
using System.Collections.Generic;
using Saves;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// TODO: Fix fact that to apply settings you need to open settings menu
namespace MainMenusScripts
{
    public class SettingsMenu : MonoBehaviour
    {

        public AudioMixer audioMixer;
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown saveDropdown;
        public Toggle fullscreenToggle;
        public Slider volumeSlider;

        private Resolution[] _resolutions;
        private List<String> saves = new();
        private GameManager _gm;

        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        void Start()
        {
            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
        
            Resolution savedRes = GetSavedResolution();
            fullscreenToggle.isOn = SimpleSaveManager.GetBool("Fullscreen");
            audioMixer.SetFloat("MasterVolume", SimpleSaveManager.GetFloat("Volume", 0f));
            volumeSlider.value = SettingsGetter.GetVolumeSliderValue();
            print(GetVolumeSliderValue());


            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                String option = _resolutions[i].width + "x" + _resolutions[i].height + "@" + _resolutions[i].refreshRate;
                options.Add(option);

                if (_resolutions[i].width == savedRes.width && 
                    _resolutions[i].height == savedRes.height && 
                    _resolutions[i].refreshRate == savedRes.refreshRate)
                {
                    currentResolutionIndex = i;
                }
            }
            
            SetResolution(currentResolutionIndex);
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            for (int i = 1; i < 4; i++)
            {
                saves.Add($"Save {i}");
            }
            
            saveDropdown.AddOptions(saves);
            saveDropdown.value = 0;
            saveDropdown.RefreshShownValue();
        }

        public void SetVolume(float volume)
        {
            float value = Mathf.Log10(volume) * 20; // Volume mixer is not linear
            audioMixer.SetFloat("MasterVolume", value); 
            SimpleSaveManager.SaveFloat("Volume", value);
        }

        public float GetVolumeSliderValue()
        {
            float audioValue = SimpleSaveManager.GetFloat("Volume", 0f);
            Debug.LogWarning($"audioValue: {audioValue}");
            return Mathf.Pow(10, audioValue / 20);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            SimpleSaveManager.SaveBool("Fullscreen", isFullscreen);
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
        
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        
            SimpleSaveManager.SaveInt("screenWidth", resolution.width);
            SimpleSaveManager.SaveInt("screenHeight", resolution.height);
            SimpleSaveManager.SaveInt("screenRefreshRate", resolution.refreshRate);
        }

        public void SetSave(int saveIndex)
        {
            _gm.currentSave = SaveManager.LoadFromJson(saves[saveIndex]);
        }

        public Resolution GetSavedResolution()
        {
            Resolution res = new Resolution();
        
            res.width = SimpleSaveManager.GetInt("screenWidth");
            res.height = SimpleSaveManager.GetInt("screenHeight");
            res.refreshRate = SimpleSaveManager.GetInt("screenRefreshRate");

            return res;
        }

        public void LoadKeybindsMenu()
        {
            // SceneManager.LoadScene....  
            // /!\ Must be in additive
        }

        public void Back()
        {
            SceneManager.UnloadSceneAsync((int)SceneBuildIndex.OptionsMenu);
        }
    
    }
}
