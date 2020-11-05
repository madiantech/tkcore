using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class RazorTemplateSuiteConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return RazorTemplateSuiteConfigFactory.REG_NAME;
            }
        }
    }
}
