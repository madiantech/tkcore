using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    internal interface IJudgeNode
    {
        NodeType NodeType { get; }

        void Execute(int level);
    }
}