using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    abstract class BaseLevelTreeConfig : IConfigCreator<ITree>
    {
        protected BaseLevelTreeConfig()
        {
        }

        #region IConfigCreator<ITree> 成员

        public ITree CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            ITableScheme scheme = TableScheme.CreateObject();

            return CreateTree(scheme, LevelTree, source);
        }

        #endregion

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(TableSchemeConfigFactory.REG_NAME)]
        public IConfigCreator<ITableScheme> TableScheme { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public LevelTreeDefinition LevelTree { get; protected set; }

        protected abstract ITree CreateTree(ITableScheme scheme,
            LevelTreeDefinition treeDef, IDbDataSource dataSource);
    }
}
