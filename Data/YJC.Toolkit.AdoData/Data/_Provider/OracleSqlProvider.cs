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
    internal sealed class OracleSqlProvider : ISqlProvider
    {
        internal const string REG_NAME = "Oracle";
        internal const string DESCRIPTION = "Oracle8i以及后续版本的数据库SQL生成器";

        private const string EXIST_TABLE = "select count(table_name) from all_tables where table_name = '{0}';";
        private const string CREATE_TABLE = "create table {0} ({1});";
        private const string DROP_TABLE = "drop table {0} cascade constraints;";
        //private const string CREATE_INDEX = "create index \"{0}_{1}\" on \"{0}\" ({2});";
        private static string[] CONVET_ARR = new string[]
        {
            "VARCHAR2({0})", "INTEGER", "DATE", "DATE", 
            "FLOAT", "CLOB", "NUMBER(8,2)", "BLOB", 
            "RAW({0})","", "XML", "SMALLINT",
            "SMALLINT", "SMALLINT", "INTEGER", "NUMBER({0},{1})"
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

            string sql;
            if (endNum == 0)
                sql = string.Format(ObjectUtil.SysCulture,
                    "SELECT {0} FROM {1} {2} {3}", selectFields, tableName, whereClause, orderBy);
            else
                sql = string.Format(ObjectUtil.SysCulture,
                    "SELECT * FROM (SELECT ROWNUM ROWNUMBER_, DD.* FROM (SELECT {0} "
                    + " FROM {1} {2} {3}) DD WHERE ROWNUM <= {4}) YY WHERE ROWNUMBER_ > {5}",
                    selectFields, tableName, whereClause, orderBy, endNum, startNum);
            return new NormalListSqlContext(sql);
        }

        string ISqlProvider.GetRowNumSql(string tableName, IFieldInfo[] keyFields, string whereClause,
            string rowNumFilter, string orderBy, int startNum, int endNum)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgument(startNum >= 0, "startNum", string.Format(ObjectUtil.SysCulture,
                "参数startNum不能为负数，现在的值为{0}", startNum), this);
            TkDebug.AssertArgument(endNum >= 0, "endNum", string.Format(ObjectUtil.SysCulture,
                "参数endNum不能为负数，现在的值为{0}", endNum), this);

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT TOOLKIT__ ROWNUMBER_ FROM (SELECT ROWNUM TOOLKIT__, DD.* FROM (SELECT * FROM ");
            sql.AppendFormat(ObjectUtil.SysCulture, "{0} {1} {2}) DD", tableName, whereClause, orderBy);
            if (endNum >= 0)
                sql.Append(" WHERE ROWNUM <= ").Append(endNum);
            sql.Append(") YY");
            if (startNum >= 0 || !string.IsNullOrEmpty(rowNumFilter))
            {
                sql.Append(" WHERE ");
                int index = 0;
                if (startNum > 0)
                    SqlBuilder.JoinStringItem(sql, index++, "TOOLKIT__ > " + startNum, " AND ");
                if (!string.IsNullOrEmpty(rowNumFilter))
                    SqlBuilder.JoinStringItem(sql, index++, rowNumFilter, " AND ");
            }
            return sql.ToString();
        }

        string ISqlProvider.GetFunction(string funcName, params object[] funcParams)
        {
            TkDebug.AssertArgumentNullOrEmpty(funcName, "funcName", this);

            switch (funcName.ToUpper(ObjectUtil.SysCulture))
            {
                case "SUBSTRING":
                    TkDebug.AssertEnumerableArgumentNull(funcParams, "funcParams", this);
                    TkDebug.AssertArgument(funcParams.Length == 3, "funcParams", string.Format(ObjectUtil.SysCulture,
                        "SUBSTRING的参数个数要求是3个，现在是{0}个", funcParams.Length), this);
                    return string.Format(ObjectUtil.SysCulture,
                        "SUBSTR({0}, {1}, {2})", funcParams);
                case "SYSDATE":
                    return "SYSDATE";
                case "LENGTH":
                    return string.Format(ObjectUtil.SysCulture, "LENGTH({0})", funcParams);
                default:
                    return string.Empty;
            }
        }

        string ISqlProvider.GetUniId(string name, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);
            TkDebug.AssertArgumentNull(context, "context", this);

            return Sequence.Execute(name, context.DbConnection);
        }

        void ISqlProvider.SetListData(IListSqlContext context, ISimpleAdapter adapter, DataSet dataSet,
            int startRecord, int maxRecords, string srcTable)
        {
            TkDebug.AssertArgumentNull(adapter, "adapter", this);
            TkDebug.AssertArgumentNull(dataSet, "dataSet", this);
            TkDebug.AssertArgumentNullOrEmpty(srcTable, "srcTable", this);

            DbUtil.FillDataSet(adapter, dataSet, srcTable);
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
            var safeFieldName = string.Empty;
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
                fieldParts.AppendFormat(ObjectUtil.SysCulture, ",\r\nconstraint \"PK_{0}\" primary key ({1})"
                    , scheme.TableName, string.Join(", ", keyFields));
            result.AppendFormat(ObjectUtil.SysCulture, CREATE_TABLE, GetSafeString(scheme.TableName), fieldParts.ToString())
               .AppendLine();
            //最后构建索引
            //foreach (var tableIndex in scheme.Indexes)
            //    sql.AppendFormat(ObjectUtil.SysCulture, CREATE_INDEX, scheme.TableName, tableIndex.Name, 
            //        (from s in tableIndex.Fields select string.Format("{0} {1}", GetSafeString(s.FieldName), s.IsAscending ? "asc" : "desc")).JoinString())
            //       .AppendLine();
            return result.ToString();
        }

        string ISqlProvider.GetDropTableSql(string tableName)
        {
            return string.Format(ObjectUtil.SysCulture, DROP_TABLE, GetSafeString(tableName));
        }

        string ISqlProvider.EscapeName(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            return string.Format(ObjectUtil.SysCulture, "\"{0}\"", name);
        }

        #endregion

        public override string ToString()
        {
            return DESCRIPTION;
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

        private static string GetSafeString(string input)
        {
            return string.Format(ObjectUtil.SysCulture, "\"{0}\"", input);
        }
    }
}
