using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal class TestQueryString : IQueryString
    {
        private readonly Dictionary<string, string> fValues = new Dictionary<string, string>();

        public TestQueryString(string name, string value)
        {
            fValues.Add(name, value);
        }

        public string this[string key]
        {
            get
            {
                if (fValues.TryGetValue(key, out string value))
                    return value;
                return null;
            }
        }

        public IEnumerable<string> AllKeys => fValues.Keys;

        public int Count => fValues.Count;

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return fValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}