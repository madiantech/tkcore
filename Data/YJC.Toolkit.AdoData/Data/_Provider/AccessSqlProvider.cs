using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CacheInstance]
    [AlwaysCache]
    [SqlProvider(Author = "YJC", CreateDate = "2009-04-10", Description = DESCRIPTION)]
    internal sealed class AccessSqlProvider : ISqlProvider
    {
        internal const string REG_NAME = "Access";
        internal const string DESCRIPTION = "Access2000及以后版本的数据库SQL生成器";

        private const string EXIST_TABLE = "select count(*) from MSYSObjects where Type = 1 AND [Name]='{0}'";
        private const string CREATE_TABLE = "create table {0} ({1});";
        private const string DROP_TABLE = "if exists (select 1 from MSYSObjects where Type = 1 AND [Name]='{0}')\r\n\t drop table {0};";
        private const string CREATE_INDEX = "create index [{0}_{1}] on [{0}] ({2});";
        private static string[] CONVET_ARR = new string[]
        {
            "TEXT({0})", "INT", "DATE", "TIME", 
            "DOUBLE", "MEMO", "CURRENCY", "LONGBINARY", 
            "GENERAL({0})","GUID", "MEMO", "BOOLEAN",
            "BYTE", "SHORT", "LONG", "NUMERIC({0},{1})"
        };

        #region ISqlProvider 成员

        IListSqlContext ISqlProvider.GetListSql(string selectFields, string tableName,
            IFieldInfo[] keyFields, string whereClause, string orderBy, int startNum, int endNum)
        {
            TkDebug.AssertArgumentNullOrEmpty(selectFields, "selectFields", this);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgument(startNum >= 0, "startNum", string.Format(ObjectUtil.SysCulture,
                "参数startNum不能为负数，现在的值为{0}", startNum), this);
            TkDebug.AssertArgument(endNum >= 0, "endNum", string.Format(ObjectUtil.SysCulture,
                "参数endNum不能为负数，现在的值为{0}", endNum), this);

            string topCount = (endNum == 0) ? string.Empty : "TOP " + endNum.ToString(ObjectUtil.SysCulture);
            string sql = string.Format(ObjectUtil.SysCulture,
                "SELECT {0} {1} FROM {2} {3} {4}", topCount,
                selectFields, tableName, whereClause, orderBy);
            return new NormalListSqlContext(sql);
        }

        string ISqlProvider.GetRowNumSql(string tableName, IFieldInfo[] keyFields,
            string whereClause, string rowNumFilter, string orderBy, int startNum, int endNum)
        {
            throw new NotSupportedException();
        }


        string ISqlProvider.GetFunction(string funcName, params object[] funcParams)
        {
            TkDebug.AssertArgumentNullOrEmpty(funcName, "funcName", this);

            switch (funcName.ToUpper(ObjectUtil.SysCulture))
            {
                case "SUBSTRING":
                    TkDebug.AssertEnumerableArgumentNull(funcParams, "funcParams", this);
                    TkDebug.AssertArgument(funcParams.Length == 3, "funcParams", string.Format(
                        ObjectUtil.SysCulture, "SUBSTRING的参数个数要求是3个，现在是{0}个",
                        funcParams.Length), this);
                    return string.Format(ObjectUtil.SysCulture,
                        "MID({0}, {1}, {2})", funcParams);
                case "SYSDATE":
                    return "NOW";
                case "LENGTH":
                    return string.Format(ObjectUtil.SysCulture, "LEN({0})", funcParams);
                default:
                    return string.Empty;
            }
        }

        string ISqlProvider.GetUniId(string name, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);
            TkDebug.AssertArgumentNull(context, "context", this);

            return SerialId.Execute(name, context);
        }

        void ISqlProvider.SetListData(IListSqlContext context, ISimpleAdapter adapter, DataSet dataSet,
            int startRecord, int maxRecords, string srcTable)
        {
            TkDebug.AssertArgumentNull(adapter, "adapter", this);
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);
            TkDebug.AssertArgumentNullOrEmpty(srcTable, "srcTable", this);
            TkDebug.AssertArgument(startRecord >= 0, "startRecord", string.Format(ObjectUtil.SysCulture,
                "参数startRecord不能为负数，现在的值为{0}", startRecord), this);
            TkDebug.AssertArgument(maxRecords >= 0, "number", string.Format(ObjectUtil.SysCulture,
                "参数maxRecords不能为负数，现在的值为{0}", maxRecords), this);

            DbUtil.FillDataSet(adapter, dataSet, srcTable, startRecord, maxRecords);
        }

        string ISqlProvider.GetExistTableSql(string tableName)
        {
            return string.Format(ObjectUtil.SysCulture, EXIST_TABLE, tableName);
        }

        string ISqlProvider.GetCreateTableSql(ITableSchemeEx scheme)
        {
            var fieldParts = new StringBuilder();
            var result = new StringBuilder();
            var keyFields = new List<string>();
            var isFirst = true;
            string safeFieldName;
            foreach (var field in scheme.Fields)
            {
                if (!isFirst)
                    fieldParts.AppendFormat(ObjectUtil.SysCulture, ",");
                else
                    isFirst = false;
                safeFieldName = GetSafeString(field.FieldName);
                fieldParts.AppendFormat(ObjectUtil.SysCulture, "\r\n{0} {1} {2}", safeFieldName, GetFieldDbType(field)
                    , field.IsEmpty ? "null" : "not null");
                if (field.IsKey)
                    keyFields.Add(safeFieldName);
            }
            if (keyFields.Count > 0)
                fieldParts.AppendFormat(ObjectUtil.SysCulture, ",\r\nconstraint [PK_{0}] primary key ({1})"
                    , scheme.TableName, string.Join(", ", keyFields));
            result.AppendFormat(ObjectUtil.SysCulture, CREATE_TABLE, GetSafeString(scheme.TableName), fieldParts.ToString())
               .AppendLine();
            //最后构建索引
            //foreach (var tableIndex in scheme.Indexes)
            //    result.AppendFormat(ObjectUtil.SysCulture, CREATE_INDEX, scheme.TableName, tableIndex.Name, string.Join(", ", 
            //                (from s in tableIndex.Fields
            //                 select string.Format(ObjectUtil.SysCulture, "{0} {1}", GetSafeString(s.FieldName), s.IsAscending ? "asc" : "desc"))))
            //       .AppendLine();
            return result.ToString();
        }

        string ISqlProvider.GetDropTableSql(string tableName)
        {
            return string.Format(ObjectUtil.SysCulture, DROP_TABLE, tableName);
        }

        string ISqlProvider.EscapeName(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            return name;
        }

        #endregion

        public override string ToString()
        {
            return DESCRIPTION;
        }

        private static string GetSafeString(string input)
        {
            return input;
            //return string.Format("[{0}]", input);
        }

        private string GetFieldDbType(IFieldInfoEx field)
        {
            var index = (int)field.DataType;
            TkDebug.Assert(index >= 0 && index < CONVET_ARR.Length
                , string.Format(ObjectUtil.SysCulture, "给定枚举值{0}越界了", field.DataType), this);
            var result = string.Format(ObjectUtil.SysCulture, CONVET_ARR[index]
                , field.Length, field.Precision);
            TkDebug.AssertNotNullOrEmpty(result
                , string.Format(ObjectUtil.SysCulture, "字段:{0}找不到相匹配的数据库的数据类型", field.FieldName), this);
            return result;
        }
    }
}
