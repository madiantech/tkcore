using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class NormalDataRowTreeNode : DataRowTreeNode
    {
        private bool fHasParent;

        public NormalDataRowTreeNode(ITree tree, DataRow row, DbTreeDefinition treeDef)
            : base(tree, row)
        {
            TkDebug.AssertArgumentNull(treeDef, "treeDef", null);

            fHasParent = true;
            Id = row[treeDef.IdField].ToString();
            Name = row[treeDef.NameField].ToString();
            ParentId = row[treeDef.ParentIdField].ToString();
            Layer = row[treeDef.LayerField].ToString();
            if (row[treeDef.LeafField].Value<int>() == 0)
            {
                HasChild = true;
                NodeType = TreeNodeType.Branch;
            }
            else
            {
                HasChild = null;
                NodeType = TreeNodeType.Leaf;
            }

            if (ParentId == tree.RootParentId)
                SetRoot();
            //switch (treeDef.SearchType)
            //{
            //    case RootSearchType.Id:
            //        if (Id == treeDef.RootId)
            //            SetRoot();
            //        break;
            //    case RootSearchType.ParentId:
            //        if (ParentId == treeDef.RootId)
            //            SetRoot();
            //        break;
            //}
        }

        public override bool HasParent
        {
            get
            {
                return fHasParent;
            }
        }

        public string Layer { get; private set; }

        [SimpleAttribute(LocalName = "type")]
        public TreeNodeType NodeType { get; private set; }

        private void SetRoot()
        {
            ParentId = "#";
            NodeType = TreeNodeType.Root;
            fHasParent = false;
        }
    }
}