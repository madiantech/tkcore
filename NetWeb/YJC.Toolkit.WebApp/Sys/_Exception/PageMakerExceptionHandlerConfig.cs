namespace YJC.Toolkit.Sys
{
    [ExceptionHandlerConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-02",
        Author = "YJC", Description = "通过配置PageMaker输出例外详细信息的Exception处理器")]
    class PageMakerExceptionHandlerConfig : IConfigCreator<IExceptionHandler>
    {
        #region IConfigCreator<IExceptionHandler> 成员

        public IExceptionHandler CreateObject(params object[] args)
        {
            IPageMaker pageMaker = PageMaker.CreateObject(args);
            return new PageMakerExceptionHandler(pageMaker) { LogException = Log };
        }

        #endregion

        [SimpleAttribute]
        public bool Log { get; internal set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(PageMakerConfigFactory.REG_NAME)]
        public IConfigCreator<IPageMaker> PageMaker { get; internal set; }
    }
}
