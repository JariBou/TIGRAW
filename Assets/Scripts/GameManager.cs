using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public SceneLoader sceneLoader;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField] 
    private PlayerData playerData;

    private void Awake()
    {
        _instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //sceneLoader = GameObject.Find("LoadingScreen").GetComponent<SceneLoader>();

    }

    public static void LoadScene(int sceneId)
    {
        GameObject gameObject = Instantiate(_instance.loadingScreen);
        _instance.sceneLoader = gameObject.GetComponent<SceneLoader>();
        _instance.sceneLoader.LoadScene(sceneId);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
