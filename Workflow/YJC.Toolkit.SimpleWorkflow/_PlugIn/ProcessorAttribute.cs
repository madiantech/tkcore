using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ProcessorAttribute : BasePlugInAttribute
    {
        public override string FactoryName
        {
            get
            {
                return ProcessorPlugInFactory.REG_NAME;
            }
        }
    }
}