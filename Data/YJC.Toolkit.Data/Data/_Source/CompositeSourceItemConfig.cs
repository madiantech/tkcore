using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class CompositeSourceItemConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public InputConditionConfig Condition { get; private set; }

        [DynamicElement(SourceConfigFactory.REG_NAME)]
        public IConfigCreator<ISource> Source { get; private set; }
    }
}
