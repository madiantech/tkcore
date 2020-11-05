using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ObjectTreeNode : ITreeNode
    {
        private readonly ITreeNode fTreeNode;
        private readonly string fParentId;

        public ObjectTreeNode(ITreeNode treeNode)
            : this(treeNode, "-1")
        {
        }

        public ObjectTreeNode(ITreeNode treeNode, string parentId)
        {
            TkDebug.AssertArgumentNull(treeNode, "treeNode", null);
            TkDebug.AssertArgumentNull(parentId, "parentId", null);

            fTreeNode = treeNode;
            if (fTreeNode.ParentId == parentId)
                fParentId = "#";
            else
                fParentId = fTreeNode.ParentId;
        }

        #region ITreeNode 成员

        [SimpleAttribute(LocalName = "children", UseSourceType = true)]
        public bool HasChildren
        {
            get
            {
                return fTreeNode.HasChildren;
            }
        }

        public bool HasParent
        {
            get
            {
                return fParentId != "#";
            }
        }

        [SimpleAttribute(LocalName = "parent")]
        public string ParentId
        {
            get
            {
                return fParentId;
            }
        }

        public ITreeNode Parent
        {
            get
            {
                return fTreeNode.Parent;
            }
        }

        public IEnumerable<ITreeNode> Children
        {
            get
            {
                return fTreeNode.Children;
            }
        }

        #endregion

        #region IEntity 成员

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Id
        {
            get
            {
                return fTreeNode.Id;
            }
        }

        [SimpleAttribute(LocalName = "text")]
        public string Name
        {
            get
            {
                return fTreeNode.Name;
            }
        }

        #endregion

        [SimpleAttribute(LocalName = "type")]
        internal TreeNodeType NodeType
        {
            get
            {
                if (fParentId == "#")
                    return TreeNodeType.Root;
                return TreeNodeType.Branch;
            }
        }
    }
}
