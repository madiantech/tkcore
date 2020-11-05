using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal abstract class BaseLevelCodeTableEasySearch : InternalTk5DbCodeTableEasySearch,
        IConfigCreator<ITree>
    {
        private readonly BaseLevelCodeTableEasySearchConfig fConfig;

        protected BaseLevelCodeTableEasySearch(BaseLevelCodeTableEasySearchConfig config)
            : base(config)
        {
            fConfig = config;
        }

        #region IConfigCreator<ITree> 成员

        public ITree CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            LevelTreeDefinition treeDef = fConfig.TreeDefinition;

            return CreateTree(SourceScheme, treeDef, source);
        }

        #endregion IConfigCreator<ITree> 成员

        protected abstract ITree CreateTree(ITableScheme scheme, LevelTreeDefinition definition,
            IDbDataSource source);
    }
}