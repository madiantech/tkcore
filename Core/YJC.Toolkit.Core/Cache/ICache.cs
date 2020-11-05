using System.Collections.Generic;

namespace YJC.Toolkit.Cache
{
    public interface ICache
    {
        object GetItem(string key, BaseCacheItemCreator creator, params object[] args);

        bool ContainsKey(string key, string cacheName);

        void Remove(string key);

        void Clear();

        IEnumerable<string> GetUnusedKeys();

        void TryRemoveList(IEnumerable<string> unusedKeys);
    }
}