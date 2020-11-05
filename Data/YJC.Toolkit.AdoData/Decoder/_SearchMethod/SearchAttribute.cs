using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SearchAttribute : BasePlugInAttribute
    {
        public SearchAttribute()
        {
            Suffix = "Search";
        }

        public override string FactoryName
        {
            get
            {
                return SearchPlugInFactory.REG_NAME;
            }
        }
    }
}
