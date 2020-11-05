using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    internal class NetUrlBuilder : IUrlBuilder
    {
        public string Build(IPageStyle style, string moduleCreator, string source,
            bool isContent = true, string handler = "c")
        {
            if (!(style is PageStyleClass styleClass))
                styleClass = PageStyleClass.FromStyle(style);
            string director = isContent ? string.Empty : "~";
            source = source.Replace('/', '_');
            if (string.Compare(moduleCreator, "xml", StringComparison.OrdinalIgnoreCase) == 0)
                return $"/{styleClass}/{director}{source}.{handler}";
            else
                return $"/{moduleCreator}/{styleClass}/{director}{source}.{handler}";
        }
    }
}