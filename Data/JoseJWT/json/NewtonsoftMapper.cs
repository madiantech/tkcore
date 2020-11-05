#if NETSTANDARD1_4

using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.JWT
{
    internal class NewtonsoftMapper : IJsonMapper
    {
        public string Serialize(object obj)
        {
            return obj.WriteJson();
        }

        public Dictionary<string, string> Parse(string json)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.ReadJson(json);
            return result;
        }
    }
}

#endif