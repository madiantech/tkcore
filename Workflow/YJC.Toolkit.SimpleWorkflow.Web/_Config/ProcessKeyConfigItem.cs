using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ProcessKeyConfigItem
    {
        [SimpleAttribute(Required = true)]
        public string NickName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string Value { get; private set; }
    }
}