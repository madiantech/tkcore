using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2020-01-09",
        Description = "对QuoteStringList的数据格式进行匹配查询")]
    internal class QuoteStringSearchConfig : IConfigCreator<BaseListSearch>
    {
        #region IConfigCreator<BaseListSearch> 成员

        public BaseListSearch CreateObject(params object[] args)
        {
            return new QuoteStringListSearch();
        }

        #endregion IConfigCreator<BaseListSearch> 成员
    }
}