using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public class Level0Tree : BaseLevelTree
    {
        public Level0Tree(ITableScheme scheme, LevelTreeDefinition levelDef, IDbDataSource source)
            : base(scheme, levelDef, source, Level0Provider.Provider)
        {
        }
    }
}
