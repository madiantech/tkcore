using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class BasePageMakerAttribute : Attribute
    {
        protected BasePageMakerAttribute()
        {
        }

        public abstract IPageMaker CreatePageMaker(IPageData pageData);
    }
}
