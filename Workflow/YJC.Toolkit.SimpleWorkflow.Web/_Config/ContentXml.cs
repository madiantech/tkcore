using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ContentXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Table")]
        public List<TableMetaDataConfig> TableList { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public WfPageMakerConfig PageMaker { get; protected set; }
    }
}