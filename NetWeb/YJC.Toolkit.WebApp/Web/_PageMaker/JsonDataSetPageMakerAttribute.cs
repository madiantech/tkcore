using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonDataSetPageMakerAttribute : BasePageMakerAttribute, IObjectFormat
    {
        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new JsonDataSetPageMaker(this);
        }

        public ConfigType GZip { get; set; }

        public ConfigType Encrypt { get; set; }
    }
}
