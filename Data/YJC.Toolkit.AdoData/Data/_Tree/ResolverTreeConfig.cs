using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [TreeConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-25",
        Description = "通过Tk5TreeTableResolver获取Tree")]
    class ResolverTreeConfig : IConfigCreator<ITree>
    {
        #region IConfigCreator<ITree> 成员

        public ITree CreateObject(params object[] args)
        {
            TableResolver resolver = Resolver.CreateObject(args);
            Tk5TreeTableResolver treeResolver = resolver.Convert<Tk5TreeTableResolver>();
            ITree tree = treeResolver.CreateTree();
            NormalDbTree dbTree = tree as NormalDbTree;
            if (dbTree != null && DataRight != null)
                dbTree.DataRight = DataRight.CreateObject(resolver);
            return tree;
        }

        #endregion

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        public IConfigCreator<TableResolver> Resolver { get; set; }

        [DynamicElement(DataRightConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IDataRight> DataRight { get; set; }
    }
}
