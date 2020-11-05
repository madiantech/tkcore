using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SqlSource : BaseDbSource
    {
        private readonly List<Tuple<string, string>> fSqlList = new List<Tuple<string, string>>();

        public SqlSource()
            : this(null, false, string.Empty)
        {
        }

        public SqlSource(string contextName, bool useCallerInfo, string successMessage)
        {
            if (!string.IsNullOrEmpty(contextName))
                Context = DbContextUtil.CreateDbContext(contextName);
            UseCallerInfo = useCallerInfo;
            SuccessMessage = successMessage;
        }

        public SqlSource(string sql, string tableName)
            : this(sql, tableName, null, false, string.Empty)
        {
        }

        public SqlSource(string sql, string tableName, string contextName,
            bool useCallerInfo, string successMessage)
            : this(contextName, useCallerInfo, successMessage)
        {
            AddItem(sql, tableName);
        }

        internal SqlSource(SqlSourceConfig config)
        {
            if (config.Context != null)
                Context = config.Context.CreateDbContext();
            if (config.Sqls != null)
                foreach (var item in config.Sqls)
                {
                    string sql = Expression.Execute(item, this);
                    AddItem(sql, item.TableName);
                }
            UseCallerInfo = config.UseCallerInfo;
            if (config.SuccessMessage != null)
                SuccessMessage = config.SuccessMessage.ToString();
        }

        public string SuccessMessage { get; protected set; }

        public bool UseCallerInfo { get; protected set; }

        public void AddItem(string sql, string tableName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            fSqlList.Add(Tuple.Create(sql, tableName));
        }

        public override OutputData DoAction(IInputData input)
        {
            using (SqlSelector selector = new SqlSelector(Context, DataSet))
            {
                foreach (var item in fSqlList)
                    selector.Select(item.Item2, item.Item1);
            }
            if (UseCallerInfo)
                input.CallerInfo.AddInfo(DataSet);

            string message = SuccessMessage ?? string.Empty;
            ActionResultData.CreateSuccessResult(message).AddToDataSet(DataSet);
            return OutputData.Create(DataSet);
        }
    }
}
