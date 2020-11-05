using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AppRightBuilderAttribute : BasePlugInAttribute
    {
        public AppRightBuilderAttribute()
        {
            Suffix = "AppRightBuilder";
        }

        public override string FactoryName
        {
            get
            {
                return AppRightBuilderPlugInFactory.REG_NAME;
            }
        }
    }
}
