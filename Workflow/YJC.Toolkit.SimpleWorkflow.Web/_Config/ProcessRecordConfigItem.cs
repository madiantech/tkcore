using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ProcessRecordConfigItem
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, Required = true, LocalName = "Key")]
        public List<ProcessKeyConfigItem> KeyList { get; private set; }
    }
}