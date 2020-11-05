using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SingleMetaDataConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return SingleMetaDataConfigFactory.REG_NAME;
            }
        }
    }
}
