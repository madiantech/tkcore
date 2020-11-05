using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SourceOutputPageMakerAttribute : BasePageMakerAttribute
    {
        public string ContentType { get; set; }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return string.IsNullOrEmpty(ContentType) ? new SourceOutputPageMaker()
                : new SourceOutputPageMaker(ContentType);
        }
    }
}
