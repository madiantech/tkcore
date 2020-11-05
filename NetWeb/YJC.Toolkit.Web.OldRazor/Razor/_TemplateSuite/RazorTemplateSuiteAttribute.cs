using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RazorTemplateSuiteAttribute : BasePlugInAttribute
    {
        public RazorTemplateSuiteAttribute()
        {
            Suffix = "RazorTemplateSuite";
        }

        public override string FactoryName
        {
            get
            {
                return RazorTemplateSuitePlugInFactory.REG_NAME;
            }
        }
    }
}
