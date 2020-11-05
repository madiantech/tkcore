using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NoPostObjectCreatorAttribute : BasePostObjectCreatorAttribute
    {
        public override IPostObjectCreator CreatePostObjectCreator(IPageData pageData)
        {
            return null;
        }
    }
}