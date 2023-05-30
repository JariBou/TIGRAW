using System;
using System.Collections;

namespace Saves.JsonDictionaryHelpers
{
    [Serializable]
    public class StringStringItem
    {
        public string key;
        public string value;
    }
    
    [Serializable]
    public class StringStringDictionary : IEnumerable
    {
        public StringStringItem[] items;
        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}