using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonObjectPageMakerAttribute : BasePageMakerAttribute, IObjectFormat
    {
        #region IObjectFormat 成员

        public ConfigType GZip { get; set; }

        public ConfigType Encrypt { get; set; }

        #endregion

        public string ModelName { get; set; }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new JsonObjectPageMaker(this);
        }
    }
}
