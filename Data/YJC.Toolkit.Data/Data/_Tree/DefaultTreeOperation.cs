using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DefaultTreeOperation : ITreeOperation
    {
        public DefaultTreeOperation(TreeOperation support, DbTreeDefinition treeDef)
            : this(support, treeDef.ExecuteRootId, treeDef.IdField, treeDef.ParentIdField)
        {
        }

        public DefaultTreeOperation(TreeOperation support, string rootId)
            : this(support, rootId, DbTreeDefinition.ID_FIELD, DbTreeDefinition.PARENT_ID_FIELD)
        {
        }

        public DefaultTreeOperation(TreeOperation support, string rootId, string idFieldName, string parentFieldName)
        {
            TkDebug.AssertArgumentNull(rootId, "rootId", null);
            TkDebug.AssertArgumentNullOrEmpty(idFieldName, "idFieldName", null);
            TkDebug.AssertArgumentNullOrEmpty(parentFieldName, "parentFieldName", null);

            Support = support;
            RootId = rootId;
            IdFieldName = idFieldName;
            ParentFieldName = parentFieldName;
        }

        #region ITreeOperation 成员

        public TreeOperation Support { get; private set; }

        public string RootId { get; private set; }

        public string IdFieldName { get; private set; }

        public string ParentFieldName { get; private set; }

        public object MoveTreeNode(string srcNodeId, string dstNodeId)
        {
            throw new NotSupportedException();
        }

        public object MoveUpDown(string nodeId, TreeNodeMoveDirection direction)
        {
            throw new NotSupportedException();
        }

        #endregion ITreeOperation 成员
    }
}