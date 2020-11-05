using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class Tk5TreeTableResolver : Tk5TableResolver, ITreeOperation
    {
        private static readonly TreeOperation Operation = TreeOperation.NewChild
            | TreeOperation.MoveNode | TreeOperation.MoveUpDown;

        private Tk5DataXml fTreeScheme;

        public Tk5TreeTableResolver(string fileName, IDbDataSource source)
            : base(fileName, source)
        {
            SetTreeScheme();
        }

        public Tk5TreeTableResolver(string fileName, string tableName, IDbDataSource source)
            : base(fileName, tableName, source)
        {
            SetTreeScheme();
        }

        public Tk5TreeTableResolver(Tk5DataXml dataXml, IDbDataSource source)
            : base(dataXml, source)
        {
            SetTreeScheme();
        }

        #region ITreeOperation 成员

        public TreeOperation Support
        {
            get
            {
                return Operation;
            }
        }

        public string RootId
        {
            get
            {
                return fTreeScheme.TreeDefinition.ExecuteRootId;
            }
        }

        public string IdFieldName
        {
            get
            {
                return fTreeScheme.TreeDefinition.IdField;
            }
        }

        public string ParentFieldName
        {
            get
            {
                return fTreeScheme.TreeDefinition.ParentIdField;
            }
        }

        public object MoveUpDown(string nodeId, TreeNodeMoveDirection direction)
        {
            TkDebug.AssertArgumentNull(nodeId, "nodeId", this);

            TreeUtil.SortTree(this, fTreeScheme.TreeDefinition, nodeId, direction);
            UpdateDatabase();
            return CreateKeyData();
        }

        public object MoveTreeNode(string srcNodeId, string dstNodeId)
        {
            TkDebug.AssertArgumentNull(srcNodeId, "srcNodeId", this);
            TkDebug.AssertArgumentNull(dstNodeId, "dstNodeId", this);

            TreeUtil.MoveTree(this, fTreeScheme.TreeDefinition, srcNodeId, dstNodeId);
            UpdateDatabase();
            return CreateKeyData();
        }

        #endregion ITreeOperation 成员

        public Tk5DataXml TreeScheme
        {
            get
            {
                return fTreeScheme;
            }
        }

        private void SetTreeScheme()
        {
            fTreeScheme = SourceSchemeEx.Convert<Tk5DataXml>();
            TkDebug.AssertNotNull(fTreeScheme.TreeDefinition, "DataXml没有定义tk:Tree", fTreeScheme);
        }

        public virtual ITree CreateTree()
        {
            return new NormalDbTree(fTreeScheme, fTreeScheme.TreeDefinition, Source, this);
        }

        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            DbTreeDefinition def = fTreeScheme.TreeDefinition;
            switch (e.Status)
            {
                case UpdateKind.Insert:
                    if (string.IsNullOrEmpty(e.Row[def.ParentIdField].ToString()))
                        e.Row[def.ParentIdField] = def.ExecuteRootId;
                    e.Row[def.LeafField] = 1;
                    e.Row[def.LayerField] = TreeUtil.GetLayer(this, def, e.Row[def.ParentIdField].ToString());
                    break;

                case UpdateKind.Update:
                    if (IsFakeDelete && e.InvokeMethod == UpdateKind.Delete)
                    {
                        if (e.Row[def.LeafField].Value<int>() == 0)
                            throw new WebPostException("该节点下还有子节点，不能删除！");
                        TreeUtil.SetParentLeaf(this, def, e.Row[def.ParentIdField].ToString());
                    }
                    break;

                case UpdateKind.Delete:
                    if (e.Row[def.LeafField].Value<int>() == 0)
                        throw new WebPostException("该节点下还有子节点，不能删除！");
                    SetCommands(AdapterCommand.Update);
                    TreeUtil.SetParentLeaf(this, def, e.Row[def.ParentIdField].ToString());
                    break;
            }
        }
    }
}