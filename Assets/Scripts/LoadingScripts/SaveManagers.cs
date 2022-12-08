using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager
{
    // For more complex saving
    private static List<Type> supportedTypes;

    public static void SaveData<T>(T data)
    {
        Type dataType = data.GetType();
        if (!supportedTypes.Contains(dataType))
        {
            throw new NotImplementedException($"Data Type '{dataType}' not supported for saving");
        }

        string savePath = Application.persistentDataPath;

        //TODO: Implement custom save
    }
    
    
}

public class SimpleSaveManager
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
