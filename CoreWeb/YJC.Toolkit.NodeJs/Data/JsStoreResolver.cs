using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class JsStoreResolver : TableResolver
    {
        public JsStoreResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("JsStore.xml"), source)
        {
        }
    }
}
