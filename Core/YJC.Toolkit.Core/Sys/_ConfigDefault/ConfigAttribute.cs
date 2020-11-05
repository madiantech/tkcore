using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ConfigAttribute : BasePlugInAttribute
    {
        public ConfigAttribute()
        {
            Suffix = "Config";
        }

        public override string FactoryName => ConfigTypeFactory.REG_NAME;
    }
}