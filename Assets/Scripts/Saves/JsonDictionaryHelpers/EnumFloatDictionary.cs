using System;

namespace Saves.JsonDictionaryHelpers
{
    [Serializable]
    public class EnumFloatItem
    {
        public string key;
        public float value;
    }
    
    [Serializable]
    public class EnumFloatDictionary
    {
        public EnumFloatItem[] items;
    }
}