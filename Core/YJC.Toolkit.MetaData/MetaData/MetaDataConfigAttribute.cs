using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class MetaDataConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return MetaDataConfigFactory.REG_NAME;
            }
        }
    }
}
