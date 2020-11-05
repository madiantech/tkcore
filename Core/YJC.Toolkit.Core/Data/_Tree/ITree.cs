using System.Collections.Generic;

namespace YJC.Toolkit.Data
{
    public interface ITree
    {
        int TopLevel { get; }

        string RootParentId { get; }

        IEnumerable<ITreeNode> RootNodes { get; }

        ITreeNode GetTreeNode(string id);

        IEnumerable<ITreeNode> GetDisplayTreeNodes(string id);

        IEnumerable<ITreeNode> GetChildNodes(string parentId);
    }
}
