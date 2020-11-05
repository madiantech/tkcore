using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public interface IUrlBuilder
    {
        string Build(IPageStyle style, string moduleCreator, string source,
            bool isContent = true, string handler = "c");
    }
}