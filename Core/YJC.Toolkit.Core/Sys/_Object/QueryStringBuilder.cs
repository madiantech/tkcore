using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public class QueryStringBuilder
    {
        private readonly Dictionary<string, QueryStringValue> fItems;
        private readonly QueryStringOutput fOutput;

        public QueryStringBuilder()
            : this(QueryStringOutput.Default)
        {
        }

        public QueryStringBuilder(QueryStringOutput output)
        {
            fItems = new Dictionary<string, QueryStringValue>();
            if (output == null)
                fOutput = QueryStringOutput.Default;
            else
                fOutput = output;
        }

        public QueryStringBuilder(string queryString)
            : this()
        {
            if (!string.IsNullOrEmpty(queryString))
                Parse(queryString);
        }

        private void Parse(string s)
        {
            int num = (s != null) ? s.Length : 0;
            for (int i = 0; i < num; i++)
            {
                int startIndex = i;
                int equalPos = -1;
                while (i < num)
                {
                    char ch = s[i];
                    if (ch == '=')
                    {
                        if (equalPos < 0)
                            equalPos = i;
                    }
                    else if (ch == '&')
                        break;
                    i++;
                }
                string name = null;
                string value = null;
                if (equalPos >= 0)
                {
                    name = s.Substring(startIndex, equalPos - startIndex);
                    value = s.Substring(equalPos + 1, (i - equalPos) - 1);
                }
                else
                {
                    name = s.Substring(startIndex, i - startIndex);
                }

                Add(name, value);
            }
        }

        public IEnumerable<string> AllKeys
        {
            get
            {
                return fItems.Keys;
            }
        }

        public QueryStringValue this[string name]
        {
            get
            {
                QueryStringValue result;
                if (fItems.TryGetValue(name, out result))
                    return result;

                return null;
            }
        }

        public void AddQueryString(string queryString)
        {
            if (!string.IsNullOrEmpty(queryString))
                Parse(queryString);
        }

        public void Add(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                return;

            name = Uri.UnescapeDataString(name);
            if (fItems.ContainsKey(name))
            {
                QueryStringValue itemValue = fItems[name];
                if (itemValue == null)
                    fItems[name] = QueryStringValue.Create(value);
                else
                    itemValue.Add(value);
            }
            else
                fItems.Add(name, QueryStringValue.Create(value));
        }

        public bool Remove(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return fItems.Remove(name);
        }

        private static string ToString(IEnumerable<KeyValuePair<string, QueryStringValue>> items)
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            foreach (var item in items)
            {
                if (index++ > 0)
                    builder.Append("&");
                builder.Append(Uri.EscapeDataString(item.Key)).Append("=");
                if (item.Value != null)
                    builder.Append(item.Value.ToString());
            }
            return builder.ToString();
        }

        public string ToString(bool sortKey, bool ignoreEmpty)
        {
            if (sortKey || ignoreEmpty)
            {
                IEnumerable<KeyValuePair<string, QueryStringValue>> items;
                if (ignoreEmpty)
                    items = from item in fItems
                            where item.Value != null
                            select item;
                else
                    items = fItems;
                if (sortKey)
                    items = from item in items
                            orderby item.Key
                            select item;
                return ToString(items);
            }
            else
                return ToString(fItems);
        }

        public override string ToString()
        {
            return ToString(fOutput.SortKey, fOutput.IgnoreEmpty);
        }
    }
}