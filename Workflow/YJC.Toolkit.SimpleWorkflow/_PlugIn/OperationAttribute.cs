using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class OperationAttribute : BasePlugInAttribute
    {
        public OperationAttribute()
        {
            Suffix = "Operation";
        }

        public override string FactoryName
        {
            get
            {
                return OperationPlugInFactory.REG_NAME;
            }
        }
    }
}
