
namespace YJC.Toolkit.Data
{
    public interface ITreeOperation
    {
        TreeOperation Support { get; }

        string RootId { get; }

        string IdFieldName { get; }

        string ParentFieldName { get; }

        object MoveTreeNode(string srcNodeId, string dstNodeId);

        object MoveUpDown(string nodeId, TreeNodeMoveDirection direction);
    }
}
