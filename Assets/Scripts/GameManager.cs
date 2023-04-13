using System;
using System.Reflection;
using LoadingScripts;
using PlayerBundle;
using PlayerBundle.PlayerUpgrades;
using Saves;
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

    public JsonSaveData currentSave;
    public PlayerUpgradesHandler PlayerUpgradesHandler;
    public RunData currentRunData; // I don't think I'll use this , nvm still need to diferenciate both
    // Put all in current save imo

    private void Awake()
    {
        _instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentRunData = new RunData();
        PlayerUpgradesHandler = new PlayerUpgradesHandler();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        Player player = Player.Instance;
        if (!player)
        {
            
            
            Debug.LogWarning("FUUUUUUUUUUUUUUUUUUUCK");
            return;
        }

        if (scene.name == "Lobby")
        {
            currentRunData.playerHealth = player.MaxHealth;
            // This is lobby so no run data theoretically
        }
        else
        {
            // this is a level
            player.health = currentRunData.playerHealth;
        }
        // player.LoadFromData(currentSave)
        
        Debug.LogWarning("FOUNDDDDDDDDDDDDDDD FUCKEEEEEEEER");
        
        
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

    public static void LoadScene(int sceneId)
    {
        // GameObject gameObject = Instantiate(_instance.loadingScreen);
        // _instance.sceneLoader = gameObject.GetComponent<SceneLoader>();
        _instance.sceneLoader.LoadScene(sceneId);
    }

    // Start is called before the first frame update

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public class RunData
{
    public int roomNumber;
    public int goldWon;
    public int crystalsWon;
    public float playerHealth;

    public RunData()
    {
        roomNumber = 0;
        goldWon = 0;
        crystalsWon = 0;
        playerHealth = 0;
    }
}
