using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class DefaultValueConfig
    {
        [SimpleAttribute(Required = true)]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public bool Force { get; private set; }

        [TextContent]
        public string Content { get; private set; }
    }
}