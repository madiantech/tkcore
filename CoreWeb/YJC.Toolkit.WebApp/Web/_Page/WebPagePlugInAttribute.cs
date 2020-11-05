using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class WebPagePlugInAttribute : BasePlugInAttribute
    {
        public WebPagePlugInAttribute()
        {
            Suffix = "Page";
        }

        public override string FactoryName
        {
            get
            {
                return WebPagePlugInFactory.REG_NAME;
            }
        }
    }
}
