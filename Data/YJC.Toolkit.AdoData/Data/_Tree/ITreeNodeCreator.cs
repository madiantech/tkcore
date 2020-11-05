using System.Data;

namespace YJC.Toolkit.Data
{
    interface ITreeNodeCreator
    {
        DataRowTreeNode CreateNode(DataRow row);
    }
}
