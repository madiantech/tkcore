using System.Collections.Generic;
using System.Collections.ObjectModel;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Collections
{
    public static class ReadOnlyDictionaryExtension
    {
        public static ReadOnlyDictionary<TKey, TValue> CreateReadOnly<TKey, TValue>(IDictionary<TKey, TValue> source)
        {
            TkDebug.AssertArgumentNull(source, "source", null);

            return new ReadOnlyDictionary<TKey, TValue>(source);
        }

        public static ReadOnlyCollection<T> CreateReadOnly<T>(IList<T> source)
        {
            TkDebug.AssertArgumentNull(source, "source", null);

            return new ReadOnlyCollection<T>(source);
        }
    }
}
