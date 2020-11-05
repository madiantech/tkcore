using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SqlAction : BaseUpdatedAction
    {
        public SqlAction(string sql)
        {
            Sql = sql;
        }

        public string Sql { get; private set; }

        protected override void Execute()
        {
            string sql = EvaluatorUtil.Execute<string>(Sql,
                ("dataSet", DataSet), ("table", Table), ("row", Row), ("resolver", Resolver));
            DbUtil.ExecuteNonQuery(sql, Resolver.Context);
        }
    }
}