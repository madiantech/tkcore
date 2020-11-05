using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    //[SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-26",
    //    Description = "支持树节点的移动，拖动，新建子节点等操作的数据源")]
    internal class TreeOperationSourceConfig : IConfigCreator<ISource>, ITreeCreator
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new TreeOperationSource(this);
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
