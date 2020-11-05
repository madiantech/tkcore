using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = true)]
    public sealed class EnumCodeTableAttribute : BasePlugInAttribute
    {
        public EnumCodeTableAttribute()
        {
            UseIntValue = true;
        }

        public bool UseIntValue { get; set; }

        public override string FactoryName
        {
            get
            {
                return "_tk_CodeTable";
            }
        }
    }
}
