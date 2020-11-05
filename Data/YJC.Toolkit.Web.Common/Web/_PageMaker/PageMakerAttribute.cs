using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PageMakerAttribute : BasePlugInAttribute
    {
        public PageMakerAttribute()
        {
            Suffix = "PageMaker";
        }

        public override string FactoryName
        {
            get
            {
                return PageMakerPlugInFactory.REG_NAME;
            }
        }
    }
}
