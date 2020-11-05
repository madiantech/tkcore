using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PageTemplateAttribute : BasePlugInAttribute
    {
        public PageTemplateAttribute()
        {
            Suffix = "PageTemplate";
        }

        public override string FactoryName
        {
            get
            {
                return PageTemplatePlugInFactory.REG_NAME;
            }
        }

        public string DefaultModelCreator { get; set; }
    }
}