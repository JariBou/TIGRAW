using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    private Resolution[] _resolutions;
    
    void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        
        Resolution savedRes = GetSavedResolution();
        fullscreenToggle.isOn = SimpleSaveManager.GetBool("Fullscreen");
        audioMixer.SetFloat("MasterVolume", SimpleSaveManager.GetFloat("Volume", 0f));
        volumeSlider.value = GetVolumeSliderValue();
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
    }

    public void SetVolume(float volume)
    {
        float value = Mathf.Log10(volume+80) * 20; // -80 offset on slider
        audioMixer.SetFloat("MasterVolume", value); // Volume mixer is not linear
        SimpleSaveManager.SaveFloat("Volume", value);
    }

    public float GetVolumeSliderValue()
    {
        float audioValue = SimpleSaveManager.GetFloat("Volume", 0f);
        Debug.LogWarning($"audioValue: {audioValue}");
        return Mathf.Pow(10, audioValue / 20) - 80;
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
        SceneManager.UnloadSceneAsync(2);
    }
    
}
