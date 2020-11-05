using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RazorEngineAttribute : BasePlugInAttribute
    {
        public RazorEngineAttribute()
        {
            Suffix = "RazorEngine";
        }

        public override string FactoryName
        {
            get
            {
                return RazorEnginePlugInFactory.REG_NAME;
            }
        }
    }
}