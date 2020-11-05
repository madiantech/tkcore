using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CoreDisplayConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return CoreDisplayConfigFactory.REG_NAME;
            }
        }
    }
}
