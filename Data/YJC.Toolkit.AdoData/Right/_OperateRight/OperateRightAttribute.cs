using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class OperateRightAttribute : BasePlugInAttribute
    {
        public OperateRightAttribute()
        {
            Suffix = "OperateRight";
        }

        public override string FactoryName
        {
            get
            {
                return OperateRightPlugInFactory.REG_NAME;
            }
        }
    }
}
