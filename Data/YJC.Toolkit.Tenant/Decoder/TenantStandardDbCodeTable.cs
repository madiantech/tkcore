using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class TenantStandardDbCodeTable : BaseSchemeCodeTable
    {
        internal const string DEFAULT_ORDER = "ORDER BY CODE_SORT, CODE_VALUE";

        private readonly CodeTableAttribute fAttribute;

        public TenantStandardDbCodeTable(string tableName)
            : base(new TenantStandardCodeTableScheme(tableName))
        {
            fAttribute = new CodeTableAttribute()
            {
                RegName = tableName,
                Description = string.Format(ObjectUtil.SysCulture, "租户标准代码表（{0}）", tableName),
                Author = ToolkitConst.TOOLKIT,
                CreateDate = DateTime.Today.ToString(ToolkitConst.DATE_FMT_STR, ObjectUtil.SysCulture)
            };
            OrderBy = DEFAULT_ORDER;
        }

        internal TenantStandardDbCodeTable(TenantStandardCodeTableConfig config)
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
            return ParamBuilder.CreateParamBuilder(
                TenantUtil.GetTenantParamBuilder(context, TenantUtil.TENANT_FIELD),
                SqlParamBuilder.CreateSql(sql));
        }
    }
}