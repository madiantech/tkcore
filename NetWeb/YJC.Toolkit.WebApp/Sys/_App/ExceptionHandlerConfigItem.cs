namespace YJC.Toolkit.Sys
{
    internal class ExceptionHandlerConfigItem
    {
        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ExceptionHanlderConfigFactory.REG_NAME)]
        public IConfigCreator<IExceptionHandler> ErrorPageException { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ExceptionHanlderConfigFactory.REG_NAME)]
        public IConfigCreator<IExceptionHandler> ReLogonException { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ExceptionHanlderConfigFactory.REG_NAME)]
        public IConfigCreator<IExceptionHandler> ErrorOperationException { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ExceptionHanlderConfigFactory.REG_NAME)]
        public IConfigCreator<IExceptionHandler> ToolkitException { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ExceptionHanlderConfigFactory.REG_NAME)]
        public IConfigCreator<IExceptionHandler> Exception { get; private set; }
    }
}
