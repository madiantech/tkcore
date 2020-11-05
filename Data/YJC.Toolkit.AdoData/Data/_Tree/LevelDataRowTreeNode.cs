using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class LevelDataRowTreeNode : DataRowTreeNode
    {
        public LevelDataRowTreeNode(ITree tree, DataRow row,
            LevelTreeDefinition treeDef, ILevelProvider provider)
            : base(tree, row)
        {
            Id = row[treeDef.IdField].ToString();
            Name = row[treeDef.NameField].ToString();
            int level = provider.GetLevel(treeDef, Id);
            ParentId = provider.GetParentId(treeDef, level, Id);
            if (level < treeDef.TotalLevel - 1)
            {
                HasChild = true;
                NodeType = level == 0 ? TreeNodeType.Root : TreeNodeType.Branch;
            }
            else
                NodeType = TreeNodeType.Leaf;
        }

        [SimpleAttribute(LocalName = "type")]
        public TreeNodeType NodeType { get; private set; }

        public override bool HasParent
        {
            get
            {
                return NodeType != TreeNodeType.Root;
            }
        }
    }
}
