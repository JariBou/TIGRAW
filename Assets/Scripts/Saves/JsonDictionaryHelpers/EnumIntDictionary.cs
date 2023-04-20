using System;

namespace Saves.JsonDictionaryHelpers
{
    [Serializable]
    public class EnumIntItem
    {
        public string key;
        public int value;
    }
    
    [Serializable]
    public class EnumIntDictionary
    {
        public EnumIntItem[] items;
    }
}