using System.Data;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class SqlCodeTable : BaseDbCodeTable, ICacheDependencyCreator
    {
        private readonly CodeTableAttribute fAttribute;
        private readonly ICacheDependency fDependency;

        public SqlCodeTable(MarcoConfigItem sql)
        {
            TkDebug.AssertArgumentNull(sql, "sql", null);

            Sql = sql;
            fDependency = NoDependency.Dependency;
        }

        internal SqlCodeTable(SqlCodeTableConfig config)
            : this(config.Sql)
        {
            fAttribute = new CodeTableAttribute(config);
            ContextName = config.Context;
            if (config.CacheDependency != null)
                fDependency = config.CacheDependency.CreateObject();
            else
                fDependency = NoDependency.Dependency;
        }

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }

        #endregion

        public MarcoConfigItem Sql { get; private set; }

        public override BasePlugInAttribute Attribute
        {
            get
            {
                return fAttribute ?? base.Attribute;
            }
        }

        protected override DataTable FillDbData(TkDbContext context, DataSet dataSet)
        {
            using (SqlSelector selector = new SqlSelector(context, dataSet))
            {
                BasePlugInAttribute attr = Attribute;
                TkDebug.AssertNotNull(attr, "需要设置CodeAttribute", this);
                string regName = attr.GetRegName(GetType());
                string sql = Expression.Execute(Sql, selector);
                selector.Select(regName, sql);
                DataTable dataTable = dataSet.Tables[regName];

                return dataTable;
            }
        }
    }
}
