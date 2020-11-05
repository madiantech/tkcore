using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class Tk5DbCodeTable : BaseSchemeCodeTable, ICacheDependencyCreator
    {
        private readonly CodeTableAttribute fAttribute;
        private readonly ICacheDependency fDependency;
        private readonly Tk5DataXml fDataXml;

        public Tk5DbCodeTable(Tk5DataXml scheme)
            : base(scheme)
        {
            fDependency = NoDependency.Dependency;
            fDataXml = scheme;
        }

        internal Tk5DbCodeTable(Tk5CodeTableConfig config)
            : base(Tk5DataXml.Create(config.DataXml))
        {
            fDataXml = (Tk5DataXml)Scheme;
            fAttribute = new CodeTableAttribute(config);
            ContextName = config.Context;
            OrderBy = config.OrderBy;
            FilterSql = config.FilterSql;
            if (!string.IsNullOrEmpty(config.NameExpression))
                NameExpression = config.NameExpression;
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

        public override BasePlugInAttribute Attribute
        {
            get
            {
                return fAttribute ?? base.Attribute;
            }
        }

        protected override IParamBuilder CreateActiveFilter(TkDbContext context)
        {
            return fDataXml.CreateParamBuilder(context, fDataXml);
        }
    }
}
