namespace YJC.Toolkit.Sys
{
    internal class WebAppExtensionXml : WebAppXml
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public WebConfigItem DefaultConfig { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public ExceptionHandlerConfigItem ExceptionHandler { get; private set; }
    }
}
