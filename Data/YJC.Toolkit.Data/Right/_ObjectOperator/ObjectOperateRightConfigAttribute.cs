using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ObjectOperateRightConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return ObjectOperateRightConfigFactory.REG_NAME;
            }
        }
    }
}
