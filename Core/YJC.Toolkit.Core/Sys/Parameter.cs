using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace YJC.Toolkit.Sys
{
    public static class Parameter
    {
        public static IParameter Create(NameValueCollection collection)
        {
            return new NameValueCollectionParameter(collection);
        }

        public static IParameter Create(IEnumerable<KeyValuePair<string, string>> dictionary)
        {
            TkDebug.AssertArgumentNull(dictionary, "dictionary", null);

            Dictionary<string, string> dict = dictionary as Dictionary<string, string>;
            if (dict == null)
            {
                dict = new Dictionary<string, string>();
                foreach (var item in dictionary)
                {
                    dict.Add(item.Key, item.Value);
                }
            }
            return new DictionaryParameter(dict);
        }

        public static IParameter Create(string name, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(name, value);
            return Create(dictionary);
        }

        public static IParameter Create(IEnumerable<string> names, IEnumerable<string> values)
        {
            TkDebug.AssertArgumentNull(names, "names", null);
            TkDebug.AssertArgumentNull(values, "values", null);

            string[] nameArray = names.ToArray();
            string[] valueArray = values.ToArray();
            TkDebug.Assert(nameArray.Length == valueArray.Length, string.Format(ObjectUtil.SysCulture,
                "参数names中的个数和参数values中的个数不匹配，names有{0}个，而values有{1}个",
                nameArray.Length, valueArray.Length), null);

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < nameArray.Length; i++)
                dictionary[nameArray[i]] = valueArray[i];

            return Create(dictionary);
        }
    }
}