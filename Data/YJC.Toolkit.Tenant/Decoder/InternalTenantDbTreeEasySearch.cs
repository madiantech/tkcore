using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class InternalTenantDbTreeEasySearch : TenantDbEasySearch, IConfigCreator<ITree>
    {
        private readonly Tk5DataXml fDataXml;

        public InternalTenantDbTreeEasySearch(Tk5TenantTreeEasySearchConfig config)
            : base(config)
        {
            fDataXml = SourceScheme.Convert<Tk5DataXml>();
        }

        #region IConfigCreator<ITree> 成员

        public ITree CreateObject(params object[] args)
        {
            if (fDataXml.TreeDefinition == null)
                return null;

            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);
            NormalDbTree dbTree = new NormalDbTree(fDataXml, fDataXml.TreeDefinition, source)
            {
                CustomCondition = TenantUtil.GetTenantParamBuilder(source.Context, TenantId)
            };
            if (DataRight != null)
                dbTree.DataRight = DataRight;
            if (FilterSql != null)
            {
                IParamBuilder builder = ParamBuilder.CreateSql(Expression.Execute(FilterSql, source));
                dbTree.CustomCondition = ParamBuilder.CreateParamBuilder(dbTree.CustomCondition, builder);
            }
            return dbTree;
        }

        #endregion IConfigCreator<ITree> 成员
    }
}