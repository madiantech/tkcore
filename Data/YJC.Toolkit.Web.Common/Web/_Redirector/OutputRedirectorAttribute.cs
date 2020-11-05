using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class OutputRedirectorAttribute : BaseRedirectorAttrbiute
    {
        public override IRedirector CreateRedirector(IPageData pageData)
        {
            return OutputRedirector.Redirector;
        }
    }
}
