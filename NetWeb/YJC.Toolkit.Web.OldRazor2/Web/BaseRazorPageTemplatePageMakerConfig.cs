using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class BaseRazorPageTemplatePageMakerConfig
    {
        [SimpleAttribute]
        public string ModelCreator { get; protected set; }

        [SimpleAttribute]
        public string RazorFile { get; protected set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(RazorDataConfigFactory.REG_NAME)]
        public IConfigCreator<object> RazorData { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Script")]
        public List<ScriptConfig> Scripts { get; protected set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Assembly")]
        public List<string> Assemblies { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public RetUrlConfig RetUrl { get; set; }
    }
}