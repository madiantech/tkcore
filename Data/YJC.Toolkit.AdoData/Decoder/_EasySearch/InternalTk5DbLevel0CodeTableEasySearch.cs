using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Decoder
{
    class InternalTk5DbLevel0CodeTableEasySearch : BaseLevelCodeTableEasySearch
    {
        public InternalTk5DbLevel0CodeTableEasySearch(Tk5Level0CodeTableEasySearchConfig config)
            : base(config)
        {
            SearchMethod = new ClassicLevelSearch(config.TreeDefinition, Level0Provider.Provider);
        }

        protected override ITree CreateTree(ITableScheme scheme, LevelTreeDefinition definition,
            IDbDataSource source)
        {
            return new Level0Tree(scheme, definition, source);
        }
    }
}
