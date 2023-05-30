using System;
using System.Collections;

namespace Saves.JsonDictionaryHelpers
{
    [Serializable]
    public class EnumFloatItem
    {
        public string key;
        public float value;
    }
    
    [Serializable]
    public class EnumFloatDictionary : IEnumerable
    {
        public EnumFloatItem[] items;
        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}