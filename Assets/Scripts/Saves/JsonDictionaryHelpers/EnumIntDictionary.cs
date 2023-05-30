using System;
using System.Collections;

namespace Saves.JsonDictionaryHelpers
{
    [Serializable]
    public class EnumIntItem
    {
        public string key;
        public int value;
    }
    
    [Serializable]
    public class EnumIntDictionary : IEnumerable
    {
        public EnumIntItem[] items;
        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}