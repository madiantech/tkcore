using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ResolverCreatorConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return ResolverCreatorConfigFactory.REG_NAME;
            }
        }
    }
}
