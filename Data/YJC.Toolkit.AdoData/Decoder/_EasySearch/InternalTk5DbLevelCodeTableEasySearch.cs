using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Decoder
{
    class InternalTk5DbLevelCodeTableEasySearch : BaseLevelCodeTableEasySearch
    {
        public InternalTk5DbLevelCodeTableEasySearch(Tk5LevelCodeTableEasySearchConfig config)
            : base(config)
        {
            SearchMethod = new ClassicLevelSearch(config.TreeDefinition, LevelProvider.Provider);
        }

        protected override ITree CreateTree(ITableScheme scheme, LevelTreeDefinition definition,
            IDbDataSource source)
        {
            return new LevelTree(scheme, definition, source);
        }
    }
}
