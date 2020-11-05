using System;
using System.Data;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class SqlSelector : IDisposable, ISqlDataAdapter, ISimpleAdapter, IDbDataSource
    {
        private IDbDataAdapter fDataAdapter;

        public SqlSelector(IDbDataSource source)
            : this(source.Context, source.DataSet)
        {
        }

        public SqlSelector(TkDbContext context, DataSet hostDataSet)
        {
            TkDebug.AssertArgumentNull(context, "context", null);
            TkDebug.AssertArgumentNull(hostDataSet, "hostDataSet", null);

            Context = context;
            HostDataSet = hostDataSet;
            //fDataAdapter = context.CreateDataAdapter();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            fDataAdapter.DisposeObject();
        }

        #endregion IDisposable 成员

        #region IDbDataSource 成员

        DataSet IDbDataSource.DataSet
        {
            get
            {
                return HostDataSet;
            }
        }

        #endregion IDbDataSource 成员

        #region ISqlDataAdapter 成员

        IDbDataAdapter ISqlDataAdapter.DataAdapter
        {
            get
            {
                return DataAdapter;
            }
        }

        #endregion ISqlDataAdapter 成员

        #region ISimpleAdapter 成员

        string ISimpleAdapter.SelectSql
        {
            get
            {
                return DataAdapter.SelectCommand == null ? string.Empty :
                    DataAdapter.SelectCommand.CommandText;
            }
        }

        void ISimpleAdapter.SetSql(string sql, IParamBuilder builder)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", this);
            //TkDebug.AssertArgumentNull(builder, "builder", this);

            SqlBuilder.GetSelectCommandSql(this, sql);
            if (builder != null)
            {
                DbParameterList paramList = builder.Parameters;
                if (!paramList.IsEmpty)
                    DbUtil.SetCommandParams(DataAdapter.SelectCommand,
                        paramList.CreateParameters(Context));
            }
        }

        int ISimpleAdapter.Fill(DataSet dataSet, string srcTable)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);
            TkDebug.AssertArgumentNullOrEmpty(srcTable, "srcTable", this);

            return DbUtil.FillDataSet(this, DataAdapter, dataSet, srcTable);
        }

        int ISimpleAdapter.Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable)
        {
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);
            TkDebug.AssertArgumentNullOrEmpty(srcTable, "srcTable", this);

            return DbUtil.FillDataSet(this, DataAdapter, dataSet, srcTable, startRecord, maxRecords);
        }

        #endregion ISimpleAdapter 成员

        private IDbDataAdapter DataAdapter
        {
            get
            {
                if (fDataAdapter == null)
                    fDataAdapter = Context.CreateDataAdapter();
                return fDataAdapter;
            }
        }

        public TkDbContext Context { get; private set; }

        public DataSet HostDataSet { get; private set; }

        public DataTable GetHostTable(string tableName)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);

            return HostDataSet.Tables[tableName];
        }

        public void Select(string tableName, string sql)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", this);

            SqlBuilder.GetSelectCommandSql(this, sql);
            DbUtil.FillDataSet(this, DataAdapter, HostDataSet, tableName);
        }

        public DataRow SelectRow(string tableName, string sql)
        {
            return DbUtil.SelectRow(() => Select(tableName, sql),
                HostDataSet, tableName);
        }

        public DataRow TrySelectRow(string tableName, string sql)
        {
            return DbUtil.TrySelectRow(() => Select(tableName, sql),
                HostDataSet, tableName);
        }

        public void Select(string tableName, string sql, params IDbDataParameter[] dbParams)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", this);
            TkDebug.AssertEnumerableArgumentNull<IDbDataParameter>(dbParams, "dbParams", this);

            SqlBuilder.GetSelectCommandSql(this, sql);
            DbUtil.SetCommandParams(DataAdapter.SelectCommand, dbParams);
            DbUtil.FillDataSet(this, DataAdapter, HostDataSet, tableName);
        }

        public DataRow SelectRow(string tableName, string sql, params IDbDataParameter[] dbParams)
        {
            return DbUtil.SelectRow(() => Select(tableName, sql, dbParams),
                HostDataSet, tableName);
        }

        public DataRow TrySelectRow(string tableName, string sql, params IDbDataParameter[] dbParams)
        {
            return DbUtil.TrySelectRow(() => Select(tableName, sql, dbParams),
                HostDataSet, tableName);
        }

        public void Select(string tableName, string sql, DbParameterList parameterList)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", this);
            TkDebug.AssertArgumentNull(parameterList, "parameterList", this);

            SqlBuilder.GetSelectCommandSql(this, sql);
            if (!parameterList.IsEmpty)
                DbUtil.SetCommandParams(DataAdapter.SelectCommand,
                    parameterList.CreateParameters(Context));
            DbUtil.FillDataSet(this, DataAdapter, HostDataSet, tableName);
        }

        public DataRow SelectRow(string tableName, string sql, DbParameterList parameterList)
        {
            return DbUtil.SelectRow(() => Select(tableName, sql, parameterList),
                HostDataSet, tableName);
        }

        public DataRow TrySelectRow(string tableName, string sql, DbParameterList parameterList)
        {
            return DbUtil.TrySelectRow(() => Select(tableName, sql, parameterList),
                HostDataSet, tableName);
        }

        public void Select(string tableName, string selectSql, IParamBuilder builder)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgumentNullOrEmpty(selectSql, "selectSql", this);
            TkDebug.AssertArgumentNull(builder, "builder", this);

            if (string.IsNullOrEmpty(builder.Sql))
                Select(tableName, selectSql);
            else
                Select(tableName, selectSql + " WHERE " + builder.Sql, builder.Parameters);
        }

        public void Select(string tableName, string selectSql, IParamBuilder builder, string orderBy)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgumentNullOrEmpty(selectSql, "selectSql", this);
            TkDebug.AssertArgumentNull(builder, "builder", this);

            StringBuilder sqlBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(builder.Sql))
                sqlBuilder.Append(" WHERE ").Append(builder.Sql);
            if (!string.IsNullOrEmpty(orderBy))
                sqlBuilder.Append(" ").Append(orderBy);
            string sql = sqlBuilder.ToString();
            sqlBuilder.Length = 0;

            if (string.IsNullOrEmpty(sql))
                Select(tableName, selectSql);
            else
                Select(tableName, selectSql + sql, builder.Parameters);
        }

        public DataRow SelectRow(string tableName, string selectSql, IParamBuilder builder)
        {
            return DbUtil.SelectRow(() => Select(tableName, selectSql, builder),
                HostDataSet, tableName);
        }

        public DataRow TrySelectRow(string tableName, string selectSql, IParamBuilder builder)
        {
            return DbUtil.TrySelectRow(() => Select(tableName, selectSql, builder),
                HostDataSet, tableName);
        }

        public override string ToString()
        {
            return Context.ContextConfig.Name + "的SqlSelector";
        }

        public static void Select(TkDbContext context, DataSet dataSet, string tableName, string sql)
        {
            using (SqlSelector selector = new SqlSelector(context, dataSet))
            {
                selector.Select(tableName, sql);
            }
        }

        public static void Select(TkDbContext context, DataSet dataSet, string tableName,
            string sql, params IDbDataParameter[] dbParams)
        {
            using (SqlSelector selector = new SqlSelector(context, dataSet))
            {
                selector.Select(tableName, sql, dbParams);
            }
        }

        public static void Select(TkDbContext context, DataSet dataSet, string tableName,
            string sql, DbParameterList parameterList)
        {
            using (SqlSelector selector = new SqlSelector(context, dataSet))
            {
                selector.Select(tableName, sql, parameterList);
            }
        }

        public static void Select(TkDbContext context, DataSet dataSet, string tableName,
            string selectSql, IParamBuilder builder)
        {
            using (SqlSelector selector = new SqlSelector(context, dataSet))
            {
                selector.Select(tableName, selectSql, builder);
            }
        }

        public static void Select(TkDbContext context, DataSet dataSet, string tableName,
            string selectSql, IParamBuilder builder, string orderBy)
        {
            using (SqlSelector selector = new SqlSelector(context, dataSet))
            {
                selector.Select(tableName, selectSql, builder, orderBy);
            }
        }
    }
}