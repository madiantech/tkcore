using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig()]
    internal class PageInfoSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new PageInfoSource();
        }

        #endregion IConfigCreator<ISource> 成员
    }
}