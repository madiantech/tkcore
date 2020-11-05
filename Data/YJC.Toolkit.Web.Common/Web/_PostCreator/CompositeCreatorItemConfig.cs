using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    class CompositeCreatorItemConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public InputConditionConfig Condition { get; private set; }

        [DynamicElement(PostObjectConfigFactory.REG_NAME)]
        public IConfigCreator<IPostObjectCreator> Creator { get; private set; }
    }
}
