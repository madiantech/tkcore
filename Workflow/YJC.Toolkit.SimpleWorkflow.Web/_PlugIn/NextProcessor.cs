using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using System.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class NextProcessor : Processor
    {
        public override IEnumerable<ResolverConfig> CreateUpdateResolverConfigs(DataRow workflowRow)
        {
            return Enumerable.Empty<ResolverConfig>();
        }
    }
}