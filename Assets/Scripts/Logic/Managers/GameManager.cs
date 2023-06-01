using System;
using System.Collections.Generic;
using LoadingScripts;
using Logic;
using PlayerBundle;
using PlayerBundle.BraceletUpgrade;
using PlayerBundle.PlayerUpgrades;
using Saves;
using Saves.JsonDictionaryHelpers;
using ScriptableObjects;
using Spells;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public enum SceneBuildIndex
{
    MainMenu = 1,
    OptionsMenu = 2,
    LoadingScene = 3,
    DeathScreen = 4,
    WinScreen = 5,
    Lobby = 6,
}

public class GameManager : MonoBehaviour
{
    [Range(1, 512)]
    public int UpdateRate = 30; // Frame update rate of enemy pathfinding set in settings according to performance

    [SerializeField]
    private GameObject _sceneLoaderObj;

    [SerializeField] public PlayerData playerData;

    public JsonSaveData currentSave; // There is always a default save
    public string currSaveName;
    
    public PlayerUpgradesHandler PlayerUpgradesHandler;
    public BraceletUpgradesHandler BraceletUpgradesHandler;
    public RunData currentRunData; // I don't think I'll use this , nvm still need to diferenciate both
    // Put all in current save imo
    
    private Player player;
    public bool isInMenu;

    private int currLevel = 0;

    // ==================================================================================================================
    public int numberOfArenas; // Number of arenas Implemented in the game
    public int numberOfBosses; // Number of Bosses implemented in the game
    // Might convert this part into an SO with all scenes inside
    public int RoomAmountBeforeBoss = 2; // number of rooms before a boss
    public int PatternNumberRep = 2; // Number of patter repetition (number of bosses) (also equals max lvl of boss)
    // ==================================================================================================================
    private RoomPathGenerator _roomPathGenerator;

    public float LocalDifficulty;
    public SpellList spellListSO;
    public List<SpellSO> baseUnlockedSkills;
    public PlayerInputsHandler PlayerInputsHandler;
    public Dictionary<String, int> spellBindings = new ();
    public Dictionary<String, SpellSO> spellBindingsSo = new ();
    public SpellBuyingHandler SpellBuyingHandler;

    [SerializeField] private SpellSO nullSpell;

    [SerializeField]
    private int NumberOfSpells = 9;

    public int heatLevelSave = 0;
    public List<StatusEffect> HeatMaluses;
    public List<GameObject> HeatMalusesDisplay;

    public UIController uiController;


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Loot.LootPickup += OnLootPickup;
        EventManager.FlagEvent += OnFlagEvent;
        SpellShopManager.SpellReassigned += OnSpellReassigned;
        BraceletUpgradeButton.OnBraceletUpgrade += OnBraceletUpgrade;
        DontDestroyOnLoad(gameObject);
        currentRunData = new RunData();
        PlayerUpgradesHandler = new PlayerUpgradesHandler();
        JsonSaveData data = null;
        try
        {
            currentSave = SaveManager.LoadFromJson("Save 1");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        BraceletUpgradesHandler = new BraceletUpgradesHandler(currentSave);
        SpellBuyingHandler = new SpellBuyingHandler(this, currentSave);

        foreach (StringStringItem KeyValuePair in currentSave.bindedSpells)
        {
            spellBindingsSo[KeyValuePair.key] = spellListSO.Find(KeyValuePair.value, nullSpell);
        }
        
        _roomPathGenerator = new RoomPathGenerator(this, RoomAmountBeforeBoss, PatternNumberRep);
        
        //Just in case
        for (int i = 0; i < NumberOfSpells; i++)
        {
            if (!spellBindingsSo.ContainsKey($"Spell {i}"))
            {
                Debug.Log($"/!\\ [Preventing Missing Values] Setting nullSpell to Spell {i}");
                spellBindingsSo[$"Spell {i}"] = nullSpell;
            }
        }
        
        // TEMP:
        // for (int i = 0; i < NumberOfSpells; i++)
        // {
        //     if (i >= spellListSO.Size)
        //     {
        //         spellBindingsSo[$"Spell {i}"] = nullSpell;
        //         continue;
        //     }
        //     Debug.Log($"Spell {i} assigned to {spellListSO[i].spellName}");
        //     spellBindingsSo[$"Spell {i}"] = spellListSO[i];
        // }
    }

    private void OnBraceletUpgrade(BraceletUpgrades obj)
    {
        if (obj == BraceletUpgrades.HealthIncrease)
        {
            playerData.health = player.GetMaxHealth();
        }
    }

    private void OnSpellReassigned(string actionName, SpellSO spellSo)
    {
        Debug.Log($"Putting {spellSo.spellName} in {actionName}");
        spellBindingsSo[actionName] = spellSo;
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
                currentSave.playerSouls += (int)Math.Round( loot.value * (1 + BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.IncreaseSoulDrop)));
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
        } else if (Input.GetKeyDown(KeyCode.F3))
        {
            EventManager.InvokeFlagEvent(Flag.EndLevel);
        }
    }

    private void ResetRun()
    {
        currLevel = 0;
        // Save stuff here
        playerData.ResetRun();
    }

    private void OnFlagEvent(Flag flag)
    {
        if (flag == Flag.EndLevel)
        {
            if (currLevel != 0) // if this was not lobby
            {
                heatLevelSave = player.heatManager.GetHeatLevel();
            }
            currLevel++;
            UpdateLocalDifficulty();
            // if (currLevel % 2 == 0)
            // {
            //     LoadScene(GetSceneBuildIndex($"Bosses/Boss{Random.Range(1, numberOfBosses+1)}_lvl1"));
            // }
            // else
            // {
            //     LoadScene(GetSceneBuildIndex($"Arenas/Level{Random.Range(1, numberOfArenas+1)}"));
            // }
            LoadScene(GetSceneBuildIndex(_roomPathGenerator.Next()));
        }  
        else if (flag == Flag.PlayerDeath)
        {
            heatLevelSave = 0;
            
            LoadScene(SceneBuildIndex.DeathScreen);
            currLevel = 0;
        }
    }

    private void UpdateLocalDifficulty()
    {
        LocalDifficulty = (currLevel * currLevel + 40 * currLevel + 70) / 100f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            uiController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
        }
        catch (Exception e)
        {
            player = null;
            Debug.LogWarning("No Player Found");
            return;
        }
        
        if(!player) {return;}

        //PlayerInputsHandler.spellLinks = spellBindings;
        
        if (scene.name == "Lobby")
        {
            GetComponent<SoundManager>().PlayRandomLobbyMusic();
            _roomPathGenerator.Regenerate();
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
        GetComponent<SoundManager>().StopMusic();
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
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.FlagEvent -= OnFlagEvent;
        Loot.LootPickup -= OnLootPickup;
        SpellShopManager.SpellReassigned -= OnSpellReassigned;
        BraceletUpgradeButton.OnBraceletUpgrade -= OnBraceletUpgrade;
    }

    public int GetSceneBuildIndex(String sceneName)
    {
        Debug.Log($"Trying to load Scene: 'Assets/Scenes/{sceneName}.unity'");
        return SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{sceneName}.unity");
    }

    private void OnApplicationQuit()
    {
        // Should Update the current save instead of creating a new JsonSaveData
        SaveManager.SaveToJson(JsonSaveData.Initialise(), currentSave.saveName);
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
