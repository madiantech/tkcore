using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ObjectContext]
    internal class ModuleTemplateConfigItem : BaseXmlPlugInItem
    {
        public const string BASE_CLASS = "_ModuleTemplate";

        [SimpleAttribute(DefaultValue = PageDataMode.Normal)]
        public PageDataMode Mode { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "PageTemplate")]
        public List<ModulePageTemplateConfigItem> PageTemplates { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "PageMaker")]
        public List<ModulePageMakerConfigItem> PageMakers { get; private set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}