using System;
using System.Collections;
using System.Collections.Generic;
using PlayerBundle.PlayerUpgrades;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class SoundManager : MonoBehaviour
{
    public bool playMusic = true;
    
    public List<SceneAudio> audios;

    private Dictionary<string, List<AudioClip>> audioDict = new();

    private AudioSource audioSource;
    private Scene currentScene; 

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        
        foreach (var sceneAudio in audios)
        {
            audioDict.Add(sceneAudio.SceneName, sceneAudio.audioSourceList);
        }


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        currentScene = scene;
        if (!playMusic) return;
        audioSource.loop = false;
        audioSource.Stop();
        if (scene.name == "Lobby")
        {
            PlayLoopingMusic(audioDict["Lobby"][0]);
        }
        // throw new NotImplementedException();
    }
    
    IEnumerator PlayLobbyMusic()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = audioDict["TestLobby"][0];
        audioSource.Play();
        audioSource.loop = true;
    }

    private void PlayLoopingMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        audioSource.loop = true;
    }
    
    IEnumerator PlayMusicOneShot()
    {
        throw new NotImplementedException();
    }
    
}

[Serializable]
public class SceneAudio
{
    public string SceneName;

    public List<AudioClip> audioSourceList;

}

[Serializable]
public class Music
{
    public bool isLoop = false;
    public AudioClip clip;
}
