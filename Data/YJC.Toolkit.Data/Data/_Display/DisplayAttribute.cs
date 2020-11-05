using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DisplayAttribute : BasePlugInAttribute
    {
        public DisplayAttribute()
        {
            Suffix = "Display";
        }

        public override string FactoryName
        {
            get
            {
                return DisplayPlugInFactory.REG_NAME;
            }
        }
    }
}
