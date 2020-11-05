using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class YukonListSqlContext : IListSqlContext
    {
        private readonly IListSqlContext fSqlContext;
        private string fSql;

        public YukonListSqlContext(IListSqlContext sqlContext)
        {
            fSqlContext = sqlContext;
            UseTopSql = true;
        }

        public YukonListSqlContext(string sql)
        {
            fSql = sql;
            UseTopSql = false;
        }

        #region IListSqlContext 成员

        string IListSqlContext.ListSql
        {
            get
            {
                return UseTopSql ? fSqlContext.ListSql : fSql;
            }
        }


        void IListSqlContext.JoinSql(string format)
        {
            TkDebug.AssertArgumentNullOrEmpty(format, "format", this);

            if (UseTopSql)
                fSqlContext.JoinSql(format);
            else
                fSql = string.Format(ObjectUtil.SysCulture, format, fSql);
        }

        #endregion

        public bool UseTopSql { get; private set; }
    }
}
