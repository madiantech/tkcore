using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public class QueryStringWrapper : IQueryString
    {
        private readonly NameValueCollection fQueryString;
        private readonly NameValueCollection fHeaders;

        public QueryStringWrapper(NameValueCollection queryString, NameValueCollection headers)
        {
            TkDebug.AssertArgumentNull(queryString, "queryString", null);
            TkDebug.AssertArgumentNull(headers, "headers", null);

            fQueryString = queryString;
            fHeaders = headers;
        }

        public string this[string key]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(key, "key", this);

                if (key.StartsWith("~", StringComparison.CurrentCulture))
                {
                    string name = key.Substring(1);
                    return fHeaders[name];
                }
                else
                    return fQueryString[key];
            }
        }

        public IEnumerable<string> AllKeys
        {
            get
            {
                return fQueryString.AllKeys;
            }
        }

        public int Count
        {
            get
            {
                return fQueryString.Count;
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var key in fQueryString.AllKeys)
                if (!string.IsNullOrEmpty(key))
                    yield return new KeyValuePair<string, string>(key, fQueryString[key]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}