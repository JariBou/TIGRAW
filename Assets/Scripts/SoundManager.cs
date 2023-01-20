using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class SoundManager : MonoBehaviour
{

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
        audioSource.Stop();
        audioSource.loop = false;
        if (scene.name == "TestLobby")
        {
            Debug.Log("Playing Audio");
            audioSource.clip = audioDict["TestLobby"][0];
            audioSource.Play();
            
            StartCoroutine(PlayLobbyMusic());
            
            
        }
        // throw new NotImplementedException();
    }
    
    IEnumerator PlayLobbyMusic()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = audioDict["TestLobby"][1];
        audioSource.Play();
        audioSource.loop = true;
    }
    
}

[Serializable]
public class SceneAudio
{
    public string SceneName;

    public List<AudioClip> audioSourceList;

}
