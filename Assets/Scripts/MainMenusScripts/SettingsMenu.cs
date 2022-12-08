using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] _resolutions;
    
    void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            String option = _resolutions[i].width + "x" + _resolutions[i].height + "@" + _resolutions[i].refreshRate;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        float value = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MasterVolume", value); // Volume mixer is not linear
        SimpleSaveManager.SaveFloat("Volume", value);
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

    public void Back()
    {
        SceneManager.LoadScene(1);
    }
    
}
