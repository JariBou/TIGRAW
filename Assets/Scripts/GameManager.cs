using System;
using System.Reflection;
using LoadingScripts;
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
        try
        {
            MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
            
            foreach (MonoBehaviour mono in sceneActive) {
                FieldInfo[] objectFields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < objectFields.Length; i++) {
                    SaveVariable attribute = Attribute.GetCustomAttribute(objectFields[i], typeof(SaveVariable)) as SaveVariable;
                    if (attribute != null)
                        Debug.Log($"{objectFields[i].Name} = {objectFields[i]}"); // The name of the flagged variable.
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
