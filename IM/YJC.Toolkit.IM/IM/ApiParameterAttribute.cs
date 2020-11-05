using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class ApiParameterAttribute : Attribute
    {
        public ApiParameterAttribute()
        {
            Location = ParamLocation.Url;
            NamingRule = NamingRule.Camel;
        }

        public ParamLocation Location { get; set; }

        public string ParamName { get; set; }

        public NamingRule NamingRule { get; set; }

        public string ModelName { get; set; }

        public string DictionaryName { get; set; }
    }
}