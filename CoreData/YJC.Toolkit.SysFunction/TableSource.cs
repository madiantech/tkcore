using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SysFunction
{
    class TableSource : EmptyDbDataSource
    {
        private const string FILTER_SQL =
            "(CF_CHARACTER = 1 OR CF_CHARACTER = 2 OR CF_CHARACTER = 3 OR CF_CHARACTER = 9 OR CF_CHARACTER = 8) OR CF_DISPLAY = 1";
        private const string ORDER_BY = "";

        private readonly CustomTableResolver fTableResolver;
        private CustomFieldResolver fFieldResolver;

        public TableSource(string context)
        {
            if (!string.IsNullOrEmpty(context))
                Context = DbContextUtil.CreateDbContext(context);

            fTableResolver = new CustomTableResolver(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fTableResolver.Dispose();
                fFieldResolver.DisposeObject();
            }

            base.Dispose(disposing);
        }

        private DataRow ReadDataRow(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return null;

            DataRow row = fTableResolver.TrySelectRowWithParam("TableName", tableName);
            return row;
        }

        public CustomTable CreateTableScheme(string tableName)
        {
            DataRow row = ReadDataRow(tableName);
            if (row == null)
                return null;

            fFieldResolver = new CustomFieldResolver(this);
            fFieldResolver.SelectWithParam(FILTER_SQL, ORDER_BY, "TableId", row["TableId"]);

            DataTable table = fFieldResolver.HostTable;
            if (table == null || table.Rows.Count == 0)
                return null;
            return new CustomTable(row, table);
        }

        //public CustomTableData ReadTableData(string tableName)
        //{
        //    DataRow row = ReadDataRow(tableName);
        //    if (row == null)
        //        return null;

        //    return row.ReadFromDataRow<CustomTableData>();
        //}
    }
}
