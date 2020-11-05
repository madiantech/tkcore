using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ControlHtmlAttribute : BasePlugInAttribute
    {
        public ControlHtmlAttribute()
        {
            Suffix = "ControlHtml";
        }

        public string SearchControl { get; set; }

        public string RangeControl { get; set; }

        public override string FactoryName
        {
            get
            {
                return ControlHtmlPlugInFactory.REG_NAME;
            }
        }
    }
}