using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TreeAttribute : BasePlugInAttribute
    {
        public TreeAttribute()
        {
            Suffix = "Tree";
        }

        public override string FactoryName
        {
            get
            {
                return TreePlugInFactory.REG_NAME;
            }
        }
    }
}
