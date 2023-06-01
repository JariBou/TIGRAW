using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public bool playMusic = true;
    
    public List<SceneAudio> audios;

    private Dictionary<string, List<AudioClip>> audioDict = new();

    public List<AudioClip> LobbyMusics;
    public List<AudioClip> ArenaMusics;
    public List<AudioClip> BossMusics;
    

    private AudioSource audioSource;
    private Scene currentScene; 

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        
        // foreach (var sceneAudio in audios)
        // {
        //     audioDict.Add(sceneAudio.SceneName, sceneAudio.audioSourceList);
        // }


        // SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayRandomLobbyMusic()
    {
        StopMusic();
        audioSource.clip = LobbyMusics[Random.Range(0, LobbyMusics.Count)];
        audioSource.Play();
        audioSource.loop = true;
    }
    
    public void PlayRandomArenaMusic()
    {
        StopMusic();
        audioSource.clip = ArenaMusics[Random.Range(0, ArenaMusics.Count)];
        audioSource.Play();
        audioSource.loop = true;
    }
    
    public void PlayRandomBossMusic()
    {
        StopMusic();
        audioSource.clip = BossMusics[Random.Range(0, BossMusics.Count)];
        audioSource.Play();
        audioSource.loop = true;
    }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    // {
    //     currentScene = scene;
    //     if (!playMusic) return;
    //     audioSource.loop = false;
    //     audioSource.Stop();
    //     if (scene.name == "Lobby")
    //     {
    //         PlayLoopingMusic(audioDict["Lobby"][0]);
    //     }
    //     // throw new NotImplementedException();
    // }
    
    IEnumerator PlayLobbyMusic()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = audioDict["TestLobby"][0];
        audioSource.Play();
        audioSource.loop = true;
    }

    public void PlayLoopingMusic(AudioClip clip)
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
