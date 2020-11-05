using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AutoProcessorAttribute : BasePlugInAttribute
    {
        public AutoProcessorAttribute()
        {
            Suffix = "AutoProcessor";
        }

        public override string FactoryName
        {
            get
            {
                return AutoProcessorPlugInFactory.REG_NAME;
            }
        }
    }
}
