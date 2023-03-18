using System;
using System.Collections.Generic;
using LoadingScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenusScripts
{
    public class SettingsGetter
    // Might keep it to aleviate InitScene.cs IF needed
    {
        public static float GetVolumeSliderValue()
        {
            float audioValue = SimpleSaveManager.GetFloat("Volume", 0f);
            Debug.LogWarning($"audioValue: {audioValue}");
            return Mathf.Pow(10, audioValue / 20);
        }
        
        public static void SetVolume(float volume, AudioMixer audioMixer)
        {
            float value = Mathf.Log10(volume) * 20; // Volume mixer is not linear
            audioMixer.SetFloat("MasterVolume", value); 
            SimpleSaveManager.SaveFloat("Volume", value);
        }

        public static void InitAudioVolume(AudioMixer audioMixer)
        {
            SetVolume(GetVolumeSliderValue(), audioMixer);
        }

    }
}