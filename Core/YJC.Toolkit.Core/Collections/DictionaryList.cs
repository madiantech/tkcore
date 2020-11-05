namespace YJC.Toolkit.Collections
{
    public class DictionaryList<TValue> : BaseDictionaryList<string, TValue> where TValue : class
    {
        public void Add(string key, TValue value)
        {
            AddItem(key, value);
        }

        public bool Contains(string key)
        {
            return ContainsKey(key);
        }
    }
}
