using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LoadingScripts;
using MainMenusScripts;
using PlayerBundle;
using PlayerBundle.BraceletUpgrade;
using PlayerBundle.PlayerUpgrades;
using Saves;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public enum SceneBuildIndex
{
    MainMenu = 1,
    OptionsMenu = 2,
    LoadingScene = 3,
    DeathScreen = 4,
    Lobby = 5,
}

public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;

    [Range(1, 512)]
    public int UpdateRate = 30; // Frame update rate of enemy pathfinding set in settings according to performance

    [SerializeField]
    private GameObject _sceneLoaderObj;

    [SerializeField] public PlayerData playerData;

    public JsonSaveData currentSave;
    public PlayerUpgradesHandler PlayerUpgradesHandler;
    public BraceletUpgradesHandler BraceletUpgradesHandler;
    public RunData currentRunData; // I don't think I'll use this , nvm still need to diferenciate both
    // Put all in current save imo
    
    private Player player;
    public bool isInMenu;

    
    private void Awake()
    {
        _instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Loot.LootPickup += OnLootPickup;
        DontDestroyOnLoad(gameObject);
        currentRunData = new RunData();
        PlayerUpgradesHandler = new PlayerUpgradesHandler();
        BraceletUpgradesHandler = new BraceletUpgradesHandler();
    }

    private void OnLootPickup(Loot loot)
    {
        if (player == null)
        {
            Debug.LogWarning("No player found");
            return;
        }
        switch (loot.type)
        {
            case LootType.Soul:
                currentSave.playerSouls += loot.value;
                break;
            case LootType.Coin:
                break;
            case LootType.Health:
                player.Heal(loot.value);
                break;
        }
    }

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentSave.playerSouls = 9999;
        }
    }

    private void ResetRun()
    {
        // Save stuff here
        playerData.ResetRun();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        catch (Exception e)
        {
            player = null;
            Debug.LogWarning("No Player Found");
            return;
        }
        
        if(!player) {return;}

        if (scene.name == "Lobby")
        {
            ResetRun();
            // This is lobby so no run data theoretically
        }

        
        
        
        //sceneLoader = GameObject.Find("LoadingScreen").GetComponent<SceneLoader>();

        #region Tests for saving

        // Tests  for saving with attributes
        // try
        // {
        //     MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
        //     
        //     foreach (MonoBehaviour mono in sceneActive) {
        //         FieldInfo[] objectFields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        //         for (int i = 0; i < objectFields.Length; i++) {
        //             SaveVariable attribute = Attribute.GetCustomAttribute(objectFields[i], typeof(SaveVariable)) as SaveVariable;
        //             if (attribute != null)
        //             {
        //                 Debug.Log(
        //                     $"[DEBUG: SaveVariable]{objectFields[i].Name} = {objectFields[i].GetValue(mono)}"); // get value of field for (object)
        //                 if (objectFields[i].Name == "heatAmount")
        //                 {
        //                     objectFields[i].SetValue(mono, 50f);
        //                 }
        //             }
        //         }
        //     }
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        // }
        //SaveManager.SaveTEST();

        #endregion

    }

    public void LoadScene(SceneBuildIndex sceneId)
    {
        LoadScene((int)sceneId);
    }
    
    public void LoadScene(int sceneId)
    {
        // GameObject gameObject = Instantiate(_instance.loadingScreen);
        // _instance.sceneLoader = gameObject.GetComponent<SceneLoader>();
        SceneManager.LoadScene((int)SceneBuildIndex.LoadingScene, LoadSceneMode.Single);
        SceneLoader sceneLoader = Instantiate(_sceneLoaderObj).GetComponent<SceneLoader>();

        sceneLoader.LoadScene(sceneId);
    }

    // Start is called before the first frame update

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public int GetSceneBuildIndex(String sceneName)
    {
        return SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{sceneName}.unity");
    }
}

[Serializable]
public class RunData
{
    public int roomNumber;
    public int goldWon;
    [FormerlySerializedAs("crystalsWon")] public int soulsWon;
    public float playerHealth;

    public RunData()
    {
        roomNumber = 0;
        goldWon = 0;
        soulsWon = 0;
        playerHealth = 0;
    }
}
