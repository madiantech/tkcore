using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public class LevelTree : BaseLevelTree
    {
        public LevelTree(ITableScheme scheme, LevelTreeDefinition levelDef, IDbDataSource source)
            : base(scheme, levelDef, source, LevelProvider.Provider)
        {
        }
    }
}
