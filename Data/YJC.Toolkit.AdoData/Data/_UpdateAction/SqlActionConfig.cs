using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [UpdatedActionConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2017-06-21",
        Author = "YJC", Description = "Evaluator表达式（可以使用dataSet,table,row,resolver四个变量），执行Sql的UpdateAction")]
    internal class SqlActionConfig : IConfigCreator<BaseUpdatedAction>
    {
        #region IConfigCreator<BaseUpdatedAction> 成员

        public BaseUpdatedAction CreateObject(params object[] args)
        {
            return new SqlAction(Sql);
        }

        #endregion IConfigCreator<BaseUpdatedAction> 成员

        [TextContent]
        public string Sql { get; private set; }
    }
}