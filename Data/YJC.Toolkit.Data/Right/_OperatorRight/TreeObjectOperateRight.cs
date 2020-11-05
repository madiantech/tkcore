using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public sealed class TreeObjectOperateRight : IObjectOperateRight
    {
        private readonly IEnumerable<string> fOtherOperators;

        public TreeObjectOperateRight()
        {
        }

        public TreeObjectOperateRight(IEnumerable<string> otherOperators)
            : this()
        {
            fOtherOperators = otherOperators;
        }

        #region IObjectOperateRight 成员

        public ObjectOperatorCollection GetOperator(ObjectOperateRightEventArgs e)
        {
            if (e.MainObj == null)
                return null;
            ObjectContainer container = e.MainObj as ObjectContainer;
            if (container == null)
                return null;
            ITreeNode node = container.MainObject as ITreeNode;
            if (node == null)
                return null;

            ObjectOperatorCollection result = new ObjectOperatorCollection(fOtherOperators);
            result.Add(RightConst.INSERT);
            result.Add(RightConst.UPDATE);

            if (DisableRootDelete)
            {
                if (node.HasParent)
                    result.Add(RightConst.DELETE);
            }
            else
                result.Add(RightConst.DELETE);

            return result;
        }

        #endregion

        public bool DisableRootDelete { get; set; }
    }
}
