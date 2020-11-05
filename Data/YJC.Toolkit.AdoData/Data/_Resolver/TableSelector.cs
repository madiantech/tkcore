using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TableSelector : IDisposable, ISqlDataAdapter, IDbDataSource, IFieldInfoIndexer,
        IRegName, IActiveData
    {
        private ITableScheme fScheme;
        private readonly ITableScheme fSourceScheme;
        private readonly IDbDataAdapter fDataAdapter;
        private DictionaryList<IFieldInfo> fKeyFieldInfos;
        private IFieldInfo[] fKeyFieldArray;

        public TableSelector(ITableScheme scheme, IDbDataSource source)
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);
            TkDebug.AssertArgumentNull(source, "source", null);

            fSourceScheme = fScheme = scheme;
            Source = source;

            Context = source.Context;
            HostDataSet = source.DataSet;
            fDataAdapter = Context.CreateDataAdapter();
            TableSchemeData schemaData = TableSchemeData.Create(Context, scheme);
            SetSchemeData(schemaData);
        }

        public TableSelector(string tableName, IDbDataSource source)
            : this(DbUtil.CreateTableScheme(tableName, source.Context), source)
        {
        }

        public TableSelector(string tableName, string keyFields, IDbDataSource source)
            : this(DbUtil.CreateTableScheme(tableName, keyFields, source.Context), source)
        {
        }

        public TableSelector(string tableName, string keyFields, string fields, IDbDataSource source)
            : this(DbUtil.CreateTableScheme(tableName, keyFields, fields, source.Context), source)
        {
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        #region IFieldInfoIndexer 成员

        public virtual IFieldInfo this[string nickName]
        {
            get
            {
                return fScheme[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return TableName;
            }
        }

        #endregion IRegName 成员

        #region IActiveData 成员

        IParamBuilder IActiveData.CreateParamBuilder(TkDbContext context, IFieldInfoIndexer indexer)
        {
            return CreateFixCondition();
        }

        #endregion IActiveData 成员

        internal IDbDataAdapter DataAdapter
        {
            get
            {
                return fDataAdapter;
            }
        }

        internal IEnumerable<IFieldInfo> FieldList
        {
            get
            {
                return fScheme.Fields;
            }
        }

        protected ITableScheme CurrentScheme
        {
            get
            {
                return fScheme;
            }
        }

        public IDbDataSource Source { get; private set; }

        public TkDbContext Context { get; private set; }

        public DataSet HostDataSet { get; private set; }

        public string TableName
        {
            get
            {
                return fScheme.TableName;
            }
        }

        public DataTable HostTable
        {
            get
            {
                return HostDataSet.Tables[TableName];
            }
        }

        public FakeDeleteInfo FakeDelete { get; set; }

        public string Fields { get; private set; }

        public virtual string ListFields
        {
            get
            {
                return Fields;
            }
        }

        public virtual string SkeletonSelectSql
        {
            get
            {
                return string.Format(ObjectUtil.SysCulture,
                    "SELECT {0} FROM {1}", Fields, Context.EscapeName(TableName));
            }
        }

        public int KeyCount { get; private set; }

        public string KeyField
        {
            get
            {
                if (KeyCount == 1)
                    return fKeyFieldInfos[0].NickName;
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "只有一个字段是主键是，才能使用此属性。现在有{0}个字段是主键", KeyCount), this);
                return null;
            }
        }

        public bool IsFakeDelete
        {
            get
            {
                return FakeDelete != null;
            }
        }

        protected internal DictionaryList<IFieldInfo> Keys
        {
            get
            {
                return fKeyFieldInfos;
            }
        }

        private void SetSchemeData(TableSchemeData schemaData)
        {
            fKeyFieldInfos = schemaData.KeyFieldInfos;
            fKeyFieldArray = schemaData.KeyFieldArray;
            Fields = schemaData.SelectFields;
            KeyCount = fKeyFieldInfos.Count;
        }

        private DataRow SelectRow(Action action)
        {
            return DbUtil.SelectRow(action, HostDataSet, TableName);
        }

        private DataRow TrySelectRow(Action action)
        {
            return DbUtil.TrySelectRow(action, HostDataSet, TableName);
        }

        private void InternalSelect(IParamBuilder builder, string orderBy)
        {
            builder = ParamBuilder.CreateParamBuilder(builder, CreateFixCondition());

            StringBuilder sqlBuilder = new StringBuilder();
            if (builder != null && !string.IsNullOrEmpty(builder.Sql))
                sqlBuilder.Append("WHERE ").Append(builder.Sql);
            if (!string.IsNullOrEmpty(orderBy))
                sqlBuilder.Append(" ").Append(orderBy);
            string sql = sqlBuilder.ToString();
            DbParameterList list = builder == null ? null : builder.Parameters;

            SqlBuilder.GetSelectCommand(this);
            IDbCommand command = DataAdapter.SelectCommand;
            command.CommandText = ReplaceWhereClause(command.CommandText, sql);
            if (list != null && !list.IsEmpty)
                DbUtil.SetCommandParams(command, list.CreateParameters(Context));
            FillDataSet();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                fDataAdapter.SelectCommand.DisposeObject();
                fDataAdapter.DisposeObject();
                HostDataSet = null;
                Context = null;
                fKeyFieldInfos = null;
                fKeyFieldArray = null;
            }
        }

        public IParamBuilder CreateFixCondition()
        {
            var builder = ParamBuilder.CreateParamBuilder(CreateFakeDeleteBuilder(),
                CreateFilterBuilder());
            return builder;
        }

        protected virtual IParamBuilder CreateFakeDeleteBuilder()
        {
            if (!IsFakeDelete)
                return null;

            return FakeDelete.CreateParamBuilder(Context, this);
        }

        protected virtual IParamBuilder CreateFilterBuilder()
        {
            return null;
        }

        protected void FillDataSet()
        {
            DbUtil.FillDataSet(this, fDataAdapter, HostDataSet, TableName);
        }

        internal IDbDataParameter CreateDbDataParameter(IFieldInfo field, bool isOrigin)
        {
            IDbDataParameter parameter = Context.CreateParameter(field, isOrigin);
            parameter.SourceColumn = field.NickName;
            if (isOrigin)
                parameter.SourceVersion = DataRowVersion.Original;
            return parameter;
        }

        protected virtual void OnMetaDataChanged(bool useSource, ITableScheme scheme)
        {
        }

        public virtual void ReadMetaData(ITableScheme metaData)
        {
            if (metaData == null)
            {
                if (fScheme != fSourceScheme)
                {
                    fScheme = fSourceScheme;
                    TableSchemeData data = TableSchemeData.Create(Context, fSourceScheme);
                    SetSchemeData(data);
                    OnMetaDataChanged(true, fScheme);
                }
            }
            else
            {
                if (fScheme != metaData)
                {
                    fScheme = metaData;
                    TableSchemeData data = new TableSchemeData(Context, fScheme);
                    SetSchemeData(data);
                    OnMetaDataChanged(false, fScheme);
                }
            }
        }

        public virtual void ReadMetaData(ITableSchemeEx metaData)
        {
            ReadMetaData(MetaDataUtil.ConvertToTableScheme(metaData));
        }

        /// <summary>
        /// 获得数据表结构
        /// </summary>
        /// <returns>空数据表</returns>
        public DataTable SelectTableStructure()
        {
            SqlBuilder.GetSelectCommand(this);
            DataAdapter.SelectCommand.CommandText = ReplaceWhereClause(
                DataAdapter.SelectCommand.CommandText, "Where 1 = 0");
            FillDataSet();
            return HostTable;
        }

        /// <summary>
        /// 得到数据表的一条空记录，如果表不存在，那么从数据库取得表结构，返回新的记录。
        /// 否则，直接从表中得到一条空记录。建议使用NewRow
        /// </summary>
        /// <returns>数据表的一条空记录</returns>
        public DataRow NewRow()
        {
            DataTable table = HostTable;
            if (table == null)
                table = SelectTableStructure();
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return row;
        }

        public void Select()
        {
            InternalSelect(null, string.Empty);
        }

        public DataRow SelectRow()
        {
            return SelectRow(() => Select());
        }

        public DataRow TrySelectRow()
        {
            return TrySelectRow(() => Select());
        }

        /// <summary>
        /// 根据SQL条件语句填充数据
        /// </summary>
        /// <param name="whereClause">SQL的条件子句</param>
        public void Select(string whereClause)
        {
            TkDebug.AssertArgumentNullOrEmpty(whereClause, "whereClause", this);

            Select(ParamBuilder.CreateSql(whereClause));
        }

        public DataRow SelectRow(string whereClause)
        {
            return SelectRow(() => Select(whereClause));
        }

        public DataRow TrySelectRow(string whereClause)
        {
            return TrySelectRow(() => Select(whereClause));
        }

        public void Select(IParamBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, "builder", this);

            InternalSelect(builder, null);
        }

        public void Select(IParamBuilder builder, string orderBy)
        {
            TkDebug.AssertArgumentNull(builder, "builder", this);

            InternalSelect(builder, orderBy);
        }

        public DataRow SelectRow(IParamBuilder builder)
        {
            return SelectRow(() => Select(builder));
        }

        public DataRow TrySelectRow(IParamBuilder builder)
        {
            return TrySelectRow(() => Select(builder));
        }

        public DataRow SelectRow(IParamBuilder builder, string orderBy)
        {
            return SelectRow(() => Select(builder, orderBy));
        }

        public DataRow TrySelectRow(IParamBuilder builder, string orderBy)
        {
            return TrySelectRow(() => Select(builder, orderBy));
        }

        /// <summary>
        /// 根据指定的表字段及字段的值来填充数据集
        /// </summary>
        /// <param name="fields">字段数组</param>
        /// <param name="values">对应的字段的值</param>
        public void SelectWithParams(string[] fields, params object[] values)
        {
            SelectWithParams(null, null, fields, values);
        }

        public IParamBuilder CreateParamBuilder(string filterSql, string[] fields, params object[] values)
        {
            TkDebug.AssertEnumerableArgumentNullOrEmpty(fields, "fields", this);
            TkDebug.AssertEnumerableArgumentNull(values, "values", this);
            TkDebug.Assert(fields.Length == values.Length, string.Format(ObjectUtil.SysCulture,
                "参数fields和values的个数不匹配，fields的个数为{0}，而values的个数为{1}",
                fields.Length, values.Length), this);

            List<IParamBuilder> list = new List<IParamBuilder>(fields.Length + 1);
            for (int i = 0; i < fields.Length; i++)
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(Context,
                    GetFieldInfo(fields[i]), values[i]);
                list.Add(builder);
            }
            if (!string.IsNullOrEmpty(filterSql))
                list.Add(ParamBuilder.CreateSql(filterSql));

            return ParamBuilder.CreateParamBuilder(list);
        }

        public void SelectWithParams(string filterSql, string orderBy, string[] fields, params object[] values)
        {
            IParamBuilder builder = CreateParamBuilder(filterSql, fields, values);
            Select(builder, orderBy);
        }

        public DataRow SelectRowWithParams(string filterSql, string orderBy, string[] fields, params object[] values)
        {
            return SelectRow(() => SelectWithParams(filterSql, orderBy, fields, values));
        }

        public DataRow SelectRowWithParams(string[] fields, params object[] values)
        {
            return SelectRow(() => SelectWithParams(fields, values));
        }

        public DataRow TrySelectRowWithParams(string filterSql, string orderBy, string[] fields, params object[] values)
        {
            return TrySelectRow(() => SelectWithParams(filterSql, orderBy, fields, values));
        }

        public DataRow TrySelectRowWithParams(string[] fields, params object[] values)
        {
            return TrySelectRow(() => SelectWithParams(fields, values));
        }

        /// <summary>
        /// 根据指定的表字段及字段的值来填充数据集
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">对应的字段的值</param>
        public void SelectWithParam(string field, object value)
        {
            TkDebug.AssertArgumentNullOrEmpty(field, "field", this);
            TkDebug.AssertArgumentNull(value, "value", this);

            Select(SqlParamBuilder.CreateEqualSql(Context, GetFieldInfo(field), value));
        }

        public void SelectWithParam(string filterSql, string orderBy, string field, object value)
        {
            TkDebug.AssertArgumentNullOrEmpty(field, "field", this);
            TkDebug.AssertArgumentNull(value, "value", this);

            IParamBuilder builder = SqlParamBuilder.CreateEqualSql(Context, GetFieldInfo(field), value);
            if (!string.IsNullOrEmpty(filterSql))
                builder = ParamBuilder.CreateParamBuilder(builder, ParamBuilder.CreateSql(filterSql));

            Select(builder, orderBy);
        }

        public DataRow SelectRowWithParam(string field, object value)
        {
            return SelectRow(() => SelectWithParam(field, value));
        }

        public DataRow TrySelectRowWithParam(string field, object value)
        {
            return TrySelectRow(() => SelectWithParam(field, value));
        }

        public void SelectWithKeys(params object[] values)
        {
            TkDebug.AssertEnumerableArgumentNull(values, "values", this);
            TkDebug.Assert(KeyCount == values.Length, string.Format(ObjectUtil.SysCulture,
                "参数values的个数和表的主键字段个数不匹配，主键的个数为{0}，而values的个数为{1}",
                KeyCount, values.Length), this);

            IParamBuilder[] builders = new IParamBuilder[KeyCount];
            for (int i = 0; i < KeyCount; ++i)
                builders[i] = SqlParamBuilder.CreateEqualSql(Context, fKeyFieldArray[i], values[i]);
            Select(ParamBuilder.CreateParamBuilder(builders));
        }

        public DataRow SelectRowWithKeys(params object[] values)
        {
            return SelectRow(() => SelectWithKeys(values));
        }

        public DataRow TrySelectRowWithKeys(params object[] values)
        {
            return TrySelectRow(() => SelectWithKeys(values));
        }

        public DataTable CreateVirtualTable(bool useAllFields = false)
        {
            DataTableCollection tables = HostDataSet.Tables;
            if (!tables.Contains(TableName))
            {
                DataTable table = DataSetUtil.CreateDataTable(TableName, useAllFields ? fScheme.AllFields : fScheme.Fields);
                tables.Add(table);
                return table;
            }
            return tables[TableName];
        }

        public string CreateUniId()
        {
            return Context.GetUniId(TableName);
        }

        public IFieldInfo GetFieldInfo(string nickName)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

            IFieldInfo result = fScheme[nickName];
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                    "在表{0}的TableSelector中，没有找到字段名为{1}的字段，请确认",
                    TableName, nickName), this);
            return result;
        }

        public string GetDisplayName(string nickName)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

            return GetFieldInfo(nickName).DisplayName;
        }

        public bool IsKey(string nickName)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

            return fKeyFieldInfos.Contains(nickName);
        }

        public IFieldInfo[] GetKeyFieldArray()
        {
            return fKeyFieldInfos.ToArray();
        }

        /// <summary>
        /// 设置表主键
        /// </summary>
        /// <param name="table">数据表</param>
        public void SetPrimaryKeys(DataTable table)
        {
            TkDebug.AssertArgumentNull(table, "table", this);

            DataColumn[] keys = Array.ConvertAll(fKeyFieldArray, field =>
            {
                DataColumn column = table.Columns[field.NickName];
                TkDebug.AssertNotNull(column, string.Format(ObjectUtil.SysCulture,
                    "表{0}中不存在字段{1}，请检查数据表是否正确", table.TableName, field.NickName), this);
                return column;
            });
            DataSetUtil.SetPrimaryKey(table, keys);
        }

        /// <summary>
        /// 查找数据行
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="sourceRow">源数据行</param>
        /// <returns>数据行</returns>
        public DataRow FindRow(DataTable table, DataRow sourceRow)
        {
            TkDebug.AssertArgumentNull(table, "table", this);
            TkDebug.AssertArgumentNull(sourceRow, "srouceRow", this);
            TkDebug.Assert(table.PrimaryKey != null && table.PrimaryKey.Length > 0,
                string.Format(ObjectUtil.SysCulture, "表{0}没有设置主键，无法执行FindRow操作",
                table.TableName), this);

            object[] keyValue = Array.ConvertAll(fKeyFieldArray, field =>
            {
                try
                {
                    return sourceRow[field.NickName];
                }
                catch (System.ArgumentException ex)
                {
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "sourceRow中没有字段{0}，请检查", field.NickName), ex, this);
                    return null;
                }
            });
            return table.Rows.Find(keyValue);
        }

        public void SelectTopRows(int topCount, IParamBuilder builder, string orderBy)
        {
            TkDebug.AssertArgumentNull(builder, "builder", this);
            TkDebug.AssertArgument(topCount > 0, "topCount", "参数必须大于0", this);

            string whereSql = builder.Sql;
            if (!string.IsNullOrEmpty(whereSql))
                whereSql = "WHERE " + whereSql;
            var listContext = Context.ContextConfig.GetListSql(ListFields, Context.EscapeName(TableName),
                GetKeyFieldArray(), whereSql, orderBy, 0, topCount);

            SqlSelector selector = new SqlSelector(Context, HostDataSet);
            using (selector)
            {
                ISimpleAdapter adapter = selector;
                adapter.SetSql(listContext.ListSql, builder);
                Context.ContextConfig.SetListData(listContext, adapter, HostDataSet, 0,
                    topCount, TableName);
            }
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "表{0}的数据读取器", TableName);
        }

        private static string ReplaceWhereClause(string sql, string whereClause)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(whereClause, "whereClause", null);

            // 陈旧代码移除 2017-10-31
            //int index = sql.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);
            //string result;
            //if (index > 0)
            //    result = sql.Substring(0, index - 1);
            //else
            //result = sql;
            string result = sql + " " + whereClause;
            return result;
        }
    }
}