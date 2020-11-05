using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class MergerConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return MergerConfigFactory.REG_NAME;
            }
        }
    }
}
