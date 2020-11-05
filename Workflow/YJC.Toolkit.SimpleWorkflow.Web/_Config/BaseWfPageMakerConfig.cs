using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class BaseWfPageMakerConfig
    {
        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(RazorDataConfigFactory.REG_NAME)]
        public IConfigCreator<object> PageData { get; set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string RazorFile { get; protected set; }
    }
}