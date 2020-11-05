using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SysFunction
{
    [TableSchemeConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-03",
        Author = "YJC", Description = "通过SYS_CUSTOM_TABLE表的定义获取单表Scheme")]
    class DbCustomTableConfig : IConfigCreator<ITableScheme>
    {
        #region IConfigCreator<ITableScheme> 成员

        public ITableScheme CreateObject(params object[] args)
        {
            using (var tableSource = new TableSource(Context))
            {
                CustomTable scheme = tableSource.CreateTableScheme(TableName);
                return scheme;
            }
        }

        #endregion

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleAttribute]
        public string TableName { get; private set; }
    }
}
