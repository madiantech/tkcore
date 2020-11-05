using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class NormalListSqlContext : IListSqlContext
    {
        private string fListSql;

        public NormalListSqlContext(string listSql)
        {
            fListSql = listSql;
        }

        #region IListSqlContext 成员

        string IListSqlContext.ListSql
        {
            get
            {
                return fListSql;
            }
        }

        void IListSqlContext.JoinSql(string format)
        {
            TkDebug.AssertArgumentNullOrEmpty(format, "format", this);

            fListSql = string.Format(ObjectUtil.SysCulture, format, fListSql);
        }

        #endregion
    }
}
