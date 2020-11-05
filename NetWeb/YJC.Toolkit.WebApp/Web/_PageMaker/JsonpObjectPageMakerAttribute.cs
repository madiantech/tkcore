using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonpObjectPageMakerAttribute : BasePageMakerAttribute
    {
        public string ModelName { get; set; }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new JsonpObjectPageMaker(this);
        }
    }
}
