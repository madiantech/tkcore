using YJC.Toolkit.Data;

namespace YJC.Toolkit.Decoder
{
    internal class InternalTk5DbSqlEasySearch : BaseSqlEasySearch
    {
        public InternalTk5DbSqlEasySearch(SqlEasySearchConfig config)
            : base(config.Sql, config.IdField, config.NameField, CreateDbContext(config.Context))
        {
            ContextName = config.Context;
            OrderBy = config.OrderBy;
            if (config.TopCount > 0)
                TopCount = config.TopCount;
            if (!string.IsNullOrEmpty(config.PyField))
                PinyinField = this[config.PyField];
            if (!string.IsNullOrEmpty(config.NameExpression))
                NameExpression = config.NameExpression;
            if (!string.IsNullOrEmpty(config.DisplayNameExpression))
                DisplayNameExpression = config.DisplayNameExpression;

            FilterSql = config.FilterSql;
            if (config.DataRight != null)
                DataRight = config.DataRight.CreateObject(this);

            SearchMethod = SearchPlugInFactory.CreateSearch(config.SearchMethod, false);
        }

        private static TkDbContext CreateDbContext(string context)
        {
            if (string.IsNullOrEmpty(context))
                return DbContextUtil.CreateDefault();
            else
                return DbContextUtil.CreateDbContext(context);
        }
    }
}