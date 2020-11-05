using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class JsonPageMakerAttribute : BasePageMakerAttribute, IObjectFormat
    {
        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new JsonPageMaker(this);
        }

        public ConfigType GZip { get; set; }

        public ConfigType Encrypt { get; set; }
    }
}