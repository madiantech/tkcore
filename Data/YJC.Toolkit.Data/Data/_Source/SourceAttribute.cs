using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SourceAttribute : BasePlugInAttribute
    {
        public SourceAttribute()
        {
            Suffix = "Source";
        }

        public override string FactoryName
        {
            get
            {
                return YJC.Toolkit.Data.SourcePlugInFactory.REG_NAME;
            }
        }
    }
}
