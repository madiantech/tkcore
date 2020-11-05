using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonPostDataSetCreatorAttribute : BasePostObjectCreatorAttribute
    {
        public override IPostObjectCreator CreatePostObjectCreator(IPageData pageData)
        {
            return JsonPostDataSetObjectCreator.Creator;
        }
    }
}
