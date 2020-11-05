using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class QueryStringWrapper : IQueryString
    {
        private readonly IQueryCollection fQueryString;

        public QueryStringWrapper(IQueryCollection queryString)
        {
            TkDebug.AssertArgumentNull(queryString, nameof(queryString), null);
            fQueryString = queryString;
        }

        public string this[string key] => fQueryString[key];

        public IEnumerable<string> AllKeys => fQueryString.Keys;

        public int Count => fQueryString.Count;

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var item in fQueryString)
                yield return new KeyValuePair<string, string>(item.Key, item.Value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}