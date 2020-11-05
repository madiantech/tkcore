using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ModulePageMakerConfigItem
    {
        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public InputConditionConfig Condition { get; private set; }

        [DynamicElement(PageMakerConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<IPageMaker> PageMaker { get; private set; }
    }
}