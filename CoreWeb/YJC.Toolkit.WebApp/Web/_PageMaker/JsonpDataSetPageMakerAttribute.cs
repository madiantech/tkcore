using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonpDataSetPageMakerAttribute : BasePageMakerAttribute
    {
        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new JsonpDataSetPageMaker();
        }
    }
}
