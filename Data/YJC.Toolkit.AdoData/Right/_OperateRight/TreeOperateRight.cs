using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class TreeOperateRight : IOperateRight
    {
        private readonly IEnumerable<string> fOtherOperators;

        public TreeOperateRight()
        {
            LayerFieldName = DbTreeDefinition.LAYER_FIELD;
        }

        public TreeOperateRight(IEnumerable<string> otherOperators)
            : this()
        {
            fOtherOperators = otherOperators;
        }

        #region IOperateRight 成员

        public IEnumerable<string> GetOperator(OperateRightEventArgs e)
        {
            if (e.Row == null)
                return null;
            List<string> result = new List<string>() { RightConst.INSERT, RightConst.UPDATE };
            if (fOtherOperators != null)
                result.AddRange(fOtherOperators);

            if (DisableRootDelete)
            {
                string layer = e.Row.GetString(LayerFieldName);
                if (layer.Length > 3)
                    result.Add(RightConst.DELETE);
            }
            else
                result.Add(RightConst.DELETE);

            return result;
        }

        #endregion

        public bool DisableRootDelete { get; set; }

        public string LayerFieldName { get; set; }
    }
}
