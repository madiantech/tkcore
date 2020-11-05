using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ModuleAttribute : BasePlugInAttribute
    {
        public ModuleAttribute()
        {
            Suffix = "Module";
        }

        public override string FactoryName
        {
            get
            {
                return ModulePlugInFactory.REG_NAME;
            }
        }
    }
}
