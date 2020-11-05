using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [TreeConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-19",
        Description = "常规Id，ParentId树形结构定义（需要符合数据库规范）")]
    class NormalDbTreeConfig : IConfigCreator<ITree>
    {
        #region IConfigCreator<ITree> 成员

        public ITree CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            ITableScheme scheme = TableScheme.CreateObject();
            NormalDbTree dbTree = new NormalDbTree(scheme, DbTree, source);
            if (DataRight != null)
                dbTree.DataRight = DataRight.CreateObject(dbTree);
            return dbTree;
        }

        #endregion

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(TableSchemeConfigFactory.REG_NAME)]
        public IConfigCreator<ITableScheme> TableScheme { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DbTreeDefinition DbTree { get; private set; }

        [DynamicElement(DataRightConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IDataRight> DataRight { get; protected set; }
    }
}
