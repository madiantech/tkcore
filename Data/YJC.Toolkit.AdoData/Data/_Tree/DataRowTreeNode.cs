using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class DataRowTreeNode : ITreeNode, IRegName
    {
        private bool fCalParent;
        private bool fCalChildren;
        private IEnumerable<ITreeNode> fChildren;
        private ITreeNode fParent;

        protected DataRowTreeNode(ITree tree, DataRow row)
        {
            TkDebug.AssertArgumentNull(tree, "tree", null);
            TkDebug.AssertArgumentNull(row, "row", null);

            Row = row;
            Tree = tree;
        }

        #region ITreeNode 成员

        public bool HasChildren
        {
            get
            {
                if (HasChild.HasValue && HasChild.Value)
                    return true;
                return false;
            }
        }

        [SimpleAttribute(LocalName = "parent")]
        public string ParentId { get; protected set; }

        public abstract bool HasParent { get; }

        public ITreeNode Parent
        {
            get
            {
                if (!fCalParent)
                {
                    fParent = Tree.GetTreeNode(ParentId);
                    fCalParent = true;
                }
                return fParent;
            }
        }

        public IEnumerable<ITreeNode> Children
        {
            get
            {
                if (!fCalChildren)
                {
                    fCalChildren = true;
                    if (HasChildren)
                        fChildren = Tree.GetChildNodes(Id);
                    else
                        fChildren = null;
                }
                return fChildren;
            }
        }

        #endregion

        #region IEntity 成员

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Id { get; protected set; }

        [SimpleAttribute(LocalName = "text")]
        public string Name { get; protected set; }

        #endregion

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Id;
            }
        }

        #endregion

        public ITree Tree { get; private set; }

        public DataRow Row { get; private set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public TreeNodeState State { get; set; }

        [SimpleAttribute(LocalName = "children", UseSourceType = true)]
        public bool? HasChild { get; set; }
    }
}
