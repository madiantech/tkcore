using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class XmlPageMakerAttribute : BasePageMakerAttribute, IObjectFormat
    {
        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new XmlPageMaker(this);
        }

        public string Root { get; set; }

        public ConfigType GZip { get; set; }

        public ConfigType Encrypt { get; set; }

        internal QName CreateRootNode()
        {
            return string.IsNullOrEmpty(Root) ? null : QName.GetFullName(Root);
        }
    }
}
