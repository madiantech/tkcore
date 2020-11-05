using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    class LevelSelector : TableSelector
    {
        public LevelSelector(ITableScheme scheme, LevelTreeDefinition levelDef, IDbDataSource source)
            : base(new LevelScheme(scheme, levelDef), source)
        {
            TreeSelector.SetFakeDelete(this, scheme);
        }
    }
}
