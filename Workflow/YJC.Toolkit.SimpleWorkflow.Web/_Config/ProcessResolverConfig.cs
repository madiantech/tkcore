using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ProcessResolverConfig : ResolverConfig
    {
        public ProcessResolverConfig(ProcessTableDataConfig config, TableResolver resolver,
            PageStyle style, UpdateKind kind, UpdateMode mode)
            : base(resolver, style, kind, mode)
        {
            Config = config;
        }

        public ProcessTableDataConfig Config { get; private set; }
    }
}