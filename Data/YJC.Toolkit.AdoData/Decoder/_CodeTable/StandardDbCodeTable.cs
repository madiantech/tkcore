using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [CacheInstance]
    [DayChangeCache]
    public class StandardDbCodeTable : BaseSchemeCodeTable
    {
        internal const string DEFAULT_ORDER = "ORDER BY CODE_SORT, CODE_VALUE";

        private readonly CodeTableAttribute fAttribute;

        public StandardDbCodeTable(string tableName)
            : base(new StandardCodeTableScheme(tableName))
        {
            fAttribute = new CodeTableAttribute();
            fAttribute.SetDefaultValue(tableName);
            OrderBy = DEFAULT_ORDER;
        }

        internal StandardDbCodeTable(StandardCodeTableConfig config)
            : base(new StandardCodeTableScheme(config.TableName))
        {
            fAttribute = new CodeTableAttribute(config);

            if (string.IsNullOrEmpty(config.OrderBy))
                OrderBy = DEFAULT_ORDER;
            else
                OrderBy = config.OrderBy;
            ContextName = config.Context;
            FilterSql = config.FilterSql;
            if (!string.IsNullOrEmpty(config.NameExpression))
                NameExpression = config.NameExpression;
        }

        public override BasePlugInAttribute Attribute
        {
            get
            {
                return fAttribute;
            }
        }

        protected override IParamBuilder CreateActiveFilter(TkDbContext context)
        {
            string sql = string.Format(ObjectUtil.SysCulture, "{0} IS NULL OR {0} <> 1",
                context.EscapeName(DecoderConst.DEL_FIELD_NAME));
            return SqlParamBuilder.CreateSql(sql);
        }
    }
}
