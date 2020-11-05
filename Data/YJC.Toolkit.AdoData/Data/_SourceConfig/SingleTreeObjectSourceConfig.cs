using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-12-03", Author = "YJC",
        Description = "提供对对象Tree模型的数据源")]
    internal class SingleTreeObjectSourceConfig : BaseTotalObjectSourceConfig, ITreeCreator
    {
        #region ITreeCreator 成员

        public string Context { get; private set; }

        public ITree CreateTree(IDbDataSource source)
        {
            return Tree.CreateObject(source);
        }

        #endregion

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(TreeConfigFactory.REG_NAME)]
        public IConfigCreator<ITree> Tree { get; private set; }

        protected override ISource CreateCustomSource(IInputData input)
        {
            return new TreeOperationSource(this);
        }

        protected override ISource CreateListSource(IInputData input)
        {
            return new TreeSource(this);
        }
    }
}
