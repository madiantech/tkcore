using System.Collections.Generic;

namespace YJC.Toolkit.Right
{
    public interface IOperateRight
    {
        IEnumerable<string> GetOperator(OperateRightEventArgs e);
    }
}
