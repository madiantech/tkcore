using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class MarcoTextPageMakerAttribute : BasePageMakerAttribute
    {
        public MarcoTextPageMakerAttribute(string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(value, "value", null);

            Value = value;
            SqlInject = true;
            EncodingName = "utf-8";
            ContentType = ContentTypeConst.HTML;
        }

        public string Value { get; private set; }

        public bool NeedParse { get; set; }

        public bool SqlInject { get; set; }

        public string EncodingName { get; set; }

        public string ContentType { get; set; }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new MarcoTextPageMaker(this);
        }
    }
}
