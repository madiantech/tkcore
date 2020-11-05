using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ObjectOperatorsConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return ObjectOperatorsConfigFactory.REG_NAME;
            }
        }
    }
}
