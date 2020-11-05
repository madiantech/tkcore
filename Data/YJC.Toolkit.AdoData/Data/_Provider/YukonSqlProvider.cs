using System.Data;
using System.Linq;
using System.Text;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CacheInstance]
    [AlwaysCache]
    [SqlProvider(RegName = REG_NAME, Author = "YJC", CreateDate = "2009-04-10", Description = DESCRIPTION)]
    internal sealed class YukonSqlProvider : SqlServerSqlProvider
    {
        internal new const string REG_NAME = "SQL Server2005";
        internal new const string DESCRIPTION = "SQL Server2005及后续版本的数据库SQL生成器";

        private static void ProcessOrderBy(IFieldInfo[] keyFields, ref string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                if (keyFields.Length == 1)
                    orderBy = "ORDER BY " + keyFields[0].FieldName;
                else
                    orderBy = "ORDER BY " + string.Join(", ", from item in keyFields select item.FieldName);
            }
        }

        protected override IListSqlContext GetListSql(string selectFields, string tableName,
            IFieldInfo[] keyFields, string whereClause, string orderBy, int startNum, int endNum)
        {
            if (keyFields == null || keyFields.Length == 0)
            {
                IListSqlContext context = base.GetListSql(selectFields, tableName, keyFields,
                    whereClause, orderBy, startNum, endNum);
                return new YukonListSqlContext(context);
            }

            string sql;
            if (endNum == 0)
                sql = string.Format(ObjectUtil.SysCulture,
                    "SELECT {0} FROM {1} {2} {3}", selectFields, tableName, whereClause, orderBy);
            else
            {
                ProcessOrderBy(keyFields, ref orderBy);

                sql = string.Format(ObjectUtil.SysCulture,
                    "SELECT * FROM (SELECT {0}, ROW_NUMBER() OVER ({3}) ROWNUMBER_ FROM {1} {2})"
                    + " _TOOLKIT WHERE ROWNUMBER_ > {4} AND ROWNUMBER_ <= {5}", selectFields, tableName, whereClause,
                    orderBy, startNum, endNum);
            }
            return new YukonListSqlContext(sql);
        }

        protected override void SetListData(IListSqlContext context, ISimpleAdapter adapter, DataSet dataSet,
            int startRecord, int maxRecords, string srcTable)
        {
            TkDebug.AssertArgumentNull(context, "context", this);

            YukonListSqlContext sqlContext = context.Convert<YukonListSqlContext>();
            if (sqlContext.UseTopSql)
                base.SetListData(context, adapter, dataSet, startRecord, maxRecords, srcTable);
            else
                DbUtil.FillDataSet(adapter, dataSet, srcTable);
        }

        protected override string GetRowNumSql(string tableName, IFieldInfo[] keyFields,
            string whereClause, string rowNumFilter, string orderBy, int startNum, int endNum)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", this);
            TkDebug.AssertArgument(!string.IsNullOrEmpty(orderBy) ||
                (keyFields != null && keyFields.Length > 0), "keyFields",
                "在参数orderBy为空时，keyFields不能为空或者是空数组", this);
            TkDebug.AssertArgument(startNum >= 0, "startNum", string.Format(ObjectUtil.SysCulture,
                "参数startNum不能为负数，现在的值为{0}", startNum), this);
            TkDebug.AssertArgument(endNum >= 0, "endNum", string.Format(ObjectUtil.SysCulture,
                "参数endNum不能为负数，现在的值为{0}", endNum), this);

            ProcessOrderBy(keyFields, ref orderBy);

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ROWNUMBER_ FROM (SELECT ROW_NUMBER() OVER (").Append(orderBy);
            sql.Append(") ROWNUMBER_, * FROM ").Append(tableName).Append(" ").Append(whereClause);
            sql.Append(") _TOOLKIT");
            if (startNum >= 0 || endNum >= 0 || !string.IsNullOrEmpty(rowNumFilter))
            {
                int index = 0;
                sql.Append(" WHERE ");
                if (startNum >= 0)
                    SqlBuilder.JoinStringItem(sql, index++, "ROWNUMBER_ > " + startNum, " AND ");
                if (endNum >= 0)
                    SqlBuilder.JoinStringItem(sql, index++, "ROWNUMBER_ <= " + endNum, " AND ");
                if (!string.IsNullOrEmpty(rowNumFilter))
                    SqlBuilder.JoinStringItem(sql, index++, rowNumFilter, " AND ");
            }
            return sql.ToString();
        }

        public override string ToString()
        {
            return DESCRIPTION;
        }
    }
}