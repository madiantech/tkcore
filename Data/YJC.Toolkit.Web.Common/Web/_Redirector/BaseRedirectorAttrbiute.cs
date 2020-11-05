using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class BaseRedirectorAttrbiute : Attribute
    {
        protected BaseRedirectorAttrbiute()
        {
        }

        public abstract IRedirector CreateRedirector(IPageData pageData);
    }
}
