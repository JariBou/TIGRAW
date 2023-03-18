using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Saves;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LoadingScripts
{
    public class SaveManager
    {
        // For more complex saving
        private static List<Type> supportedTypes = new List<Type>()
        {                             // TODO: check if Dictionary<dynamic, dynamic> == Dictionary<smthElse, smthElse>
            typeof(Vector2), typeof(Vector3), typeof(int), typeof(string), typeof(Dictionary<dynamic, dynamic>) 
        };

        private static Dictionary<int, string> test;

        public static void SaveData<T>(T data)
        {
            Type dataType = data.GetType();
            if (!supportedTypes.Contains(dataType))
            {
                if (dataType.Name == typeof(Dictionary<dynamic, dynamic>).Name)
                {
                    Debug.Log($"It's ok for {dataType}");
                }
                else
                {
                    throw new NotImplementedException($"Data Type '{dataType}' not supported for saving");
                }
            }
            Debug.Log($"It's ok for {dataType}");
            string savePath = Application.persistentDataPath;

            //TODO: Implement custom save
        }

        public static void SaveToJson(JsonSaveData data)
        {
            string jsonData = JsonUtility.ToJson(data);
            string filepath = Application.persistentDataPath + "/PlayerUpgrades.json";
            Debug.LogWarning(jsonData);
            File.WriteAllText(filepath, jsonData);
        }
        
        public static void SaveToJson(JsonSaveData data, string filename)
        {
            string jsonData = JsonUtility.ToJson(data);
            string filepath = Application.persistentDataPath + "/" + filename + (filename.EndsWith(".json") ? ""
                : ".json");
            Debug.LogWarning(jsonData);
            File.WriteAllText(filepath, jsonData);
        }

        public static JsonSaveData LoadFromJson()
        {
            string filepath = Application.persistentDataPath + "/PlayerUpgrades.json";
            string jsonData = File.ReadAllText(filepath);
            Debug.LogWarning(jsonData);

            JsonSaveData data = JsonUtility.FromJson<JsonSaveData>(jsonData);
            return data;
        }

        public static void Save(string saveName, object saveData)
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            
            if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            }

            string path = Application.persistentDataPath + "/saves/" + saveName + ".save";

            FileStream file = File.Create(path);

            formatter.Serialize(file, saveData);

        }

        public static void SaveTEST()
        {
            /*
             * Save only upgrades and don't modify base values of Player class
             * Make methods to get the player stats with upgrades
             * Affected by arrays of enum of upgrades to know which upgrades to look for
             */
            Hashtable saveData = new Hashtable(); // HAsh tables are serializable ("yay")
            try
            {
                MonoBehaviour[] sceneActive = Object.FindObjectsOfType<MonoBehaviour>();
            
                foreach (MonoBehaviour mono in sceneActive) {
                    FieldInfo[] objectFields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                    for (int i = 0; i < objectFields.Length; i++) {
                        SaveVariable attribute = Attribute.GetCustomAttribute(objectFields[i], typeof(SaveVariable)) as SaveVariable;
                        if (attribute != null)
                        {
                            saveData.Add(objectFields[i].Name, objectFields[i].GetValue(mono));
                            Debug.Log(
                                $"[DEBUG: SaveVariable]{objectFields[i].Name} = {objectFields[i].GetValue(mono)}"); // get value of field for (object)
                            if (objectFields[i].Name == "heatAmount")
                            {
                                objectFields[i].SetValue(mono, 50f);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            SurrogateSelector selector = new SurrogateSelector();

            Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
            
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);

            binaryFormatter.SurrogateSelector = selector;
            
            return binaryFormatter;
        }
    }

    public static class SimpleSaveManager
    {
        public static void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
    
        public static void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
    
        public static void SaveBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    
        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public static int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }
    
        public static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }
    
        public static float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }
    
        public static bool GetBool(string key)
        {
            return PlayerPrefs.GetInt(key) == 1;
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }
    
    
    }
}