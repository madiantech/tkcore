using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class SqlTableResolver : MetaDataTableResolver
    {
        private readonly string fSql;

        public SqlTableResolver(string sql, ITableSchemeEx scheme,
            IDbDataSource source)
            : base(scheme, source)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);

            ReadOnly = true;
            fSql = sql;
        }

        public override string SkeletonSelectSql
        {
            get
            {
                return string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1}",
                    Fields, SqlTableName);
            }
        }

        public string SqlTableName
        {
            get
            {
                return string.Format(ObjectUtil.SysCulture, "({0}) {1}", fSql,
                    Context.EscapeName(TableName));
            }
        }

        public override string GetSqlTableName(TkDbContext context)
        {
            return SqlTableName;
        }
    }
}