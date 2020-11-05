using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class CompositePageMakerItemConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public InputConditionConfig Condition { get; private set; }

        [DynamicElement(PageMakerConfigFactory.REG_NAME)]
        public IConfigCreator<IPageMaker> PageMaker { get; private set; }
    }
}
