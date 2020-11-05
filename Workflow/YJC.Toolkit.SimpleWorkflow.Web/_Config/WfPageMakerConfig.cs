using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WfPageMakerConfig : BaseWfPageMakerConfig
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Script")]
        public List<ScriptConfig> Scripts { get; private set; }
    }
}