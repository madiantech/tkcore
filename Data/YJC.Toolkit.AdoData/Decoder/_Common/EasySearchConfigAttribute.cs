using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class EasySearchConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return EasySearchConfigFactory.REG_NAME;
            }
        }
    }
}
