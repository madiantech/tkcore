using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CreatorAttribute : BasePlugInAttribute
    {
        public CreatorAttribute()
        {
            Suffix = "Creator";
        }

        public override string FactoryName
        {
            get
            {
                return CreatorPlugInFactory.REG_NAME;
            }
        }
    }
}
