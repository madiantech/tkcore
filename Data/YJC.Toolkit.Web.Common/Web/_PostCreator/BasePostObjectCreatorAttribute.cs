using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class BasePostObjectCreatorAttribute : Attribute
    {
        protected BasePostObjectCreatorAttribute()
        {
        }

        public abstract IPostObjectCreator CreatePostObjectCreator(IPageData pageData);
    }
}
