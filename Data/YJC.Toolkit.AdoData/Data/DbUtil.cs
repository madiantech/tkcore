using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using YJC.Toolkit.MetaData;

//using YJC.Toolkit.Decoder;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static partial class DbUtil
    {
        //private readonly static Type BlobType = typeof(byte[]);

        internal static int FillDataSet(ISimpleAdapter adapter, DataSet dataSet, string tableName)
        {
            TkTrace.LogInfo(adapter.SelectSql);
            return adapter.Fill(dataSet, tableName);
        }

        internal static int FillDataSet(ISimpleAdapter adapter, DataSet dataSet, string tableName,
            int startRecord, int maxRecords)
        {
            TkTrace.LogInfo(adapter.SelectSql);
            return adapter.Fill(dataSet, startRecord, maxRecords, tableName);
        }

        internal static int FillDataSet(object sender, IDbDataAdapter adapter, DataSet dataSet, string tableName)
        {
            TkDebug.AssertNotNull(adapter.SelectCommand, "adapter参数的SelectCommand为空，无法从数据库取数据", sender);
            TkDebug.AssertNotNullOrEmpty(adapter.SelectCommand.CommandText,
                "adapter参数的SelectCommand的CommandText属性为空，无法从数据库取数据", sender);

            TkTrace.LogInfo(adapter.SelectCommand.CommandText);
            try
            {
                return (adapter as DbDataAdapter).Fill(dataSet, tableName);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "执行SQL:{0}时出错", adapter.SelectCommand.CommandText), ex, sender);
                return -1;
            }
        }

        internal static int FillDataSet(object sender, IDbDataAdapter adapter, DataSet dataSet, string tableName,
            int startRecord, int maxRecords)
        {
            TkDebug.AssertNotNull(adapter.SelectCommand, "adapter参数的SelectCommand为空，无法从数据库取数据", sender);
            TkDebug.AssertNotNullOrEmpty(adapter.SelectCommand.CommandText,
                "adapter参数的SelectCommand的CommandText属性为空，无法从数据库取数据", sender);

            TkTrace.LogInfo(adapter.SelectCommand.CommandText);
            try
            {
                return (adapter as DbDataAdapter).Fill(dataSet, startRecord, maxRecords, tableName);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "执行SQL:{0}时出错", adapter.SelectCommand.CommandText), ex, sender);
                return -1;
            }
        }

        internal static DataRow SelectRow(Action action, DataSet dataSet, string tableName)
        {
            DataTable table = dataSet.Tables[tableName];
            int count = table == null ? 0 : table.Rows.Count;
            action();
            table = dataSet.Tables[tableName];
            TkDebug.AssertNotNull(table, string.Format(ObjectUtil.SysCulture,
                "DataSet中没有表{0}，请检查是否填充了数据", tableName), null);
            TkDebug.Assert(table.Rows.Count > count, string.Format(ObjectUtil.SysCulture,
                "数据表{0}应该至少选出一条记录，但实际一条都没有。", tableName), null);
            return table.Rows[count];
        }

        internal static DataRow TrySelectRow(Action action, DataSet dataSet, string tableName)
        {
            DataTable table = dataSet.Tables[tableName];
            int count = table == null ? 0 : table.Rows.Count;
            action();
            table = dataSet.Tables[tableName];
            if (table == null || table.Rows.Count <= count)
                return null;
            return table.Rows[count];
        }

        internal static void SetCommandParams(IDbCommand command, IDbDataParameter[] args)
        {
            command.Parameters.Clear();
            foreach (IDbDataParameter param in args)
                command.Parameters.Add(param);
        }

        internal static bool IsInputParameter(ParameterDirection direction)
        {
            return direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput;
        }

        internal static bool IsOutputParameter(ParameterDirection direction)
        {
            return direction != ParameterDirection.Input;
        }

        public static ITableScheme CreateSqlTableScheme(string sql, string tableName,
            string keyFields, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNullOrEmpty(keyFields, "keyFields", null);
            TkDebug.AssertArgumentNull(context, "context", null);

            IDbDataAdapter adapter = context.CreateDataAdapter();
            DataSet dataSet = new DataSet { Locale = ObjectUtil.SysCulture };
            using (dataSet)
            {
                IDbCommand command = context.CreateCommand();
                adapter.SelectCommand = command;
                using (adapter as IDisposable)
                using (command)
                {
                    string newSql = string.Format(ObjectUtil.SysCulture,
                        "SELECT * FROM ({0}) {1} WHERE 1 = 0", sql, tableName);
                    command.CommandText = newSql;
                    try
                    {
                        FillDataSet(null, adapter, dataSet, tableName);
                    }
                    catch (Exception ex)
                    {
                        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            "无法选取数据库表{0}，请确认该表是否存在于数据库中", tableName), ex, null);
                    }
                }
                return new InternalTableScheme(tableName, keyFields, dataSet.Tables[tableName]);
            }
        }

        public static ITableScheme CreateTableScheme(string tableName, string keyFields,
            string fields, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNullOrEmpty(keyFields, "keyFields", null);
            TkDebug.AssertArgumentNullOrEmpty(fields, "fields", null);
            TkDebug.AssertArgumentNull(context, "context", null);

            IDbDataAdapter adapter = context.CreateDataAdapter();
            DataSet dataSet = new DataSet { Locale = ObjectUtil.SysCulture };
            using (dataSet)
            {
                CreateTableScheme(tableName, fields, context, adapter, dataSet);
                return new InternalTableScheme(tableName, keyFields, dataSet.Tables[tableName]);
            }
        }

        public static ITableScheme CreateTableScheme(string tableName, string keyFields, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNullOrEmpty(keyFields, "keyFields", null);
            TkDebug.AssertArgumentNull(context, "context", null);

            IDbDataAdapter adapter = context.CreateDataAdapter();
            DataSet dataSet = new DataSet { Locale = ObjectUtil.SysCulture };
            using (dataSet)
            {
                CreateTableScheme(tableName, "*", context, adapter, dataSet);
                return new InternalTableScheme(tableName, keyFields, dataSet.Tables[tableName]);
            }
        }

        public static ITableScheme CreateTableScheme(string tableName, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNull(context, "context", null);

            IDbDataAdapter adapter = context.CreateDataAdapter();
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            DataSet dataSet = new DataSet { Locale = ObjectUtil.SysCulture };
            using (dataSet)
            {
                CreateTableScheme(tableName, "*", context, adapter, dataSet);
                return new InternalTableScheme(tableName, dataSet.Tables[tableName]);
            }
        }

        private static void CreateTableScheme(string tableName, string fields, TkDbContext context,
            IDbDataAdapter adapter, DataSet dataSet)
        {
            IDbCommand command = context.CreateCommand();
            adapter.SelectCommand = command;

            using (adapter as IDisposable)
            using (command)
            {
                command.CommandText = string.Format(ObjectUtil.SysCulture, "SELECT {1} FROM {0} WHERE 1 = 0", tableName, fields);
                try
                {
                    FillDataSet(null, adapter, dataSet, tableName);
                }
                catch (Exception ex)
                {
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "无法选取数据库表{0}，请确认该表是否存在于数据库中", tableName), ex, null);
                }
            }
        }

        ///// <summary>
        ///// 在内存中创建标准的代码表，包括CODE_VALUE和CODE_NAME两个字段
        ///// </summary>
        ///// <param name="codeTableName">需要创建的代码表表名</param>
        ///// <returns>创建好的的代码表</returns>
        //public static DataTable CreateCodeTable(string codeTableName)
        //{
        //    TkDebug.AssertArgumentNullOrEmpty(codeTableName, "codeTableName", null);

        //    DataTable table = CreateDataTable(codeTableName, DecoderConst.CODE_FIELD_NAME, DecoderConst.NAME_FIELD_NAME);
        //    return table;
        //}

        public static bool ExistTable(string tableName, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            return ExecuteScalar(context.ContextConfig.InternalSqlProvider.GetExistTableSql(tableName),
                context).Value<bool>();
        }

        public static void CreateTable(ITableSchemeEx scheme, TkDbContext context)
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);
            ExecuteScalar(context.ContextConfig.InternalSqlProvider.GetCreateTableSql(scheme), context);
        }

        public static void DropTable(string tableName, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            ExecuteScalar(context.ContextConfig.InternalSqlProvider.GetDropTableSql(tableName), context);
        }

        public static string GetCondition(MetaDataTableResolver resolver, string nickName, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);
            TkDebug.AssertArgumentNull(value, "value", null);

            return GetCondition(resolver, Tuple.Create(nickName, value));
        }

        public static string GetCondition(MetaDataTableResolver resolver, params Tuple<string, string>[] conditions)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(conditions, "conditions", null);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in conditions)
                dict[item.Item1] = item.Item2;

            QueryConditionObject obj = new QueryConditionObject(true, dict);
            IParamBuilder builder = resolver.GetQueryCondition(obj);
            QueryCondition condition = new QueryCondition(dict, builder);
            return condition.ToEncodeString();
        }

        //public static int SaveError(string refTable, string refId, string moduleName,
        //    string createId, Exception ex)
        //{
        //    using (DbContext context = AbstractGlobalVariable.Current.
        //        InternalDefaultDbContextConfig.CreateDbContext())
        //    {
        //        return SaveError(context, refTable, refId, moduleName, createId, ex);
        //    }
        //}

        //public static int SaveError(DbContext context, string refTable, string refId,
        //    string moduleName, string createId, Exception ex)
        //{
        //    TkDebug.AssertArgumentNull(context, "context", null);
        //    TkDebug.AssertArgumentNullOrEmpty(moduleName, "moduleName", null);
        //    TkDebug.AssertArgumentNull(ex, "ex", null);

        //    DataSet ds = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
        //    using (ds)
        //    {
        //        SysErrorTableResolver resolver = new SysErrorTableResolver(context, ds);
        //        using (resolver)
        //        {
        //            return resolver.SaveException(refTable, refId, moduleName, createId, ex);
        //        }
        //    }
        //}

        //public static string ParamBuilderToXml(IParamBuilder builder)
        //{
        //    TkDebug.AssertArgumentNull(builder, "builder", null);

        //    return XmlParamBuilder.ToString(builder);
        //}

        //public static IParamBuilder XmlToParamBuilder(string xml)
        //{
        //    TkDebug.AssertArgumentNullOrEmpty(xml, "xml", null);

        //    return XmlParamBuilder.FromString(xml);
        //}
    }
}