using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SysFunction
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-10-24", Description = "通过SYS_CUSTOM_TABLE表的定义获取产生的TableResolver")]
    class DbCustomTableResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            using (var tableSource = new TableSource(Context))
            {
                CustomTable scheme = tableSource.CreateTableScheme(TableName);
                if (scheme == null)
                    return null;

                IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);
                TableResolver resolver = new MetaDataTableResolver(scheme, source);
                CustomTableData data = scheme.TableData;
                if (data.Extension1 != null)
                {
                    resolver.AutoTrackField = data.Extension1.AutoTrackField;
                    resolver.AutoUpdateKey = data.Extension1.AutoUpdateKey;
                }

                return resolver;
            }
        }

        #endregion

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleAttribute]
        public string TableName { get; private set; }
    }
}
