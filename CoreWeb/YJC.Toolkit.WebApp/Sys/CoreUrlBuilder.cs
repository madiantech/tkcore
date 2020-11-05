using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    internal class CoreUrlBuilder : IUrlBuilder
    {
        public string Build(IPageStyle style, string moduleCreator, string source,
            bool isContent = true, string handler = "c")
        {
            if (!(style is PageStyleClass styleClass))
                styleClass = PageStyleClass.FromStyle(style);
            string director = isContent ? string.Empty : "~";
            return $"/{handler}/{director}{moduleCreator}/{styleClass}/{source}";
        }
    }
}