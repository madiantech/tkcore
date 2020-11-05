namespace YJC.Toolkit.Sys
{
    internal sealed class WebConfigItem
    {
        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(PostObjectConfigFactory.REG_NAME)]
        public IConfigCreator<IPostObjectCreator> DefaultPostObjectCreator { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(PageMakerConfigFactory.REG_NAME)]
        public IConfigCreator<IPageMaker> DefaultPageMaker { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(RedirectorConfigFactory.REG_NAME)]
        public IConfigCreator<IRedirector> DefaultRedirector { get; private set; }

        [ObjectElement]
        public ReadSettings ReadSettings { get; private set; }

        [ObjectElement]
        public WriteSettings WriteSettings { get; private set; }
    }
}