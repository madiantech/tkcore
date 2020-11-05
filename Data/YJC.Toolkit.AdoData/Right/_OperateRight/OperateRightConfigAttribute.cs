using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class OperateRightConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return OperateRightConfigFactory.REG_NAME;
            }
        }
    }
}
