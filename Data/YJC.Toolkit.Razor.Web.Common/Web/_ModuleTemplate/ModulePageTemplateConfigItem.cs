using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ModulePageTemplateConfigItem
    {
        [SimpleAttribute(Required = true)]
        public string Template { get; private set; }

        [SimpleAttribute]
        public string ModelCreator { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public InputConditionConfig Condition { get; private set; }
    }
}