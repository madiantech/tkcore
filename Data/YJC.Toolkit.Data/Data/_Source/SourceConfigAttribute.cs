using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SourceConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return SourceConfigFactory.REG_NAME;
            }
        }
    }
}
