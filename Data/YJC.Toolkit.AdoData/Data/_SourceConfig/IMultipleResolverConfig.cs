using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IMultipleResolverConfig
    {
        IConfigCreator<TableResolver> MainResolver { get; }

        IEnumerable<ChildTableInfoConfig> ChildResolvers { get; }
    }
}