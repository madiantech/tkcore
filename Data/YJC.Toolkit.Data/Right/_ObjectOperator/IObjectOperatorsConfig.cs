using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public interface IObjectOperatorsConfig
    {
        IConfigCreator<IObjectOperateRight> Right { get; }

        IEnumerable<OperatorConfig> Operators { get; }
    }
}
