using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class PageInitData : IPageTemplateInitData
    {
        public static IPageTemplateInitData Instance = new PageInitData();

        private PageInitData()
        {
        }

        public string PageTemplateName { get; }

        public string ModelCreatorName { get => "DataSetEdit"; }
    }
}