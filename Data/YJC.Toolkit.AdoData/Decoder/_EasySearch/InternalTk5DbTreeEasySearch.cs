using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class InternalTk5DbTreeEasySearch : Tk5DbEasySearch, IConfigCreator<ITree>
    {
        private readonly Tk5DataXml fDataXml;

        public InternalTk5DbTreeEasySearch(BaseEasySearchConfig config)
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
            NormalDbTree dbTree = new NormalDbTree(fDataXml, fDataXml.TreeDefinition, source);
            if (DataRight != null)
                dbTree.DataRight = DataRight;
            if (FilterSql != null)
                dbTree.CustomCondition = ParamBuilder.CreateSql(Expression.Execute(FilterSql, source.Context, source));
            return dbTree;
        }

        #endregion IConfigCreator<ITree> 成员
    }
}