using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-08-19", Description = "树形结构的数据源")]
    class TreeSourceConfig : IConfigCreator<ISource>, ITreeCreator
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            TreeSource source = new TreeSource(this);
            return source;
        }

        #endregion

        #region ITreeCreator 成员

        [SimpleAttribute]
        public string Context { get; private set; }

        public ITree CreateTree(IDbDataSource source)
        {
            return Tree.CreateObject(source);
        }

        #endregion

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(TreeConfigFactory.REG_NAME)]
        public IConfigCreator<ITree> Tree { get; private set; }
    }
}
