using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

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

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            
            
            
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