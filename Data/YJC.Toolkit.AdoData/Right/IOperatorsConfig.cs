using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public interface IOperatorsConfig
    {
        IConfigCreator<IOperateRight> Right { get; }

        IEnumerable<OperatorConfig> Operators { get; }
    }
}
