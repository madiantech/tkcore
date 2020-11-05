using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RazorBaseTemplateAttribute : BasePlugInAttribute
    {
        public RazorBaseTemplateAttribute()
        {
            Suffix = "Template";
        }

        public override string FactoryName
        {
            get
            {
                return RazorBaseTemplatePlugInFactory.REG_NAME;
            }
        }
    }
}