using System;
using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    internal class WebDefaultXmlConfig : BaseDefaultValue, IReadObjectCallBack
    {
        private class TempPageMakerConfig : IConfigCreator<IPageMaker>
        {
            private readonly IPageMaker fPageMaker;

            public TempPageMakerConfig(IPageMaker pageMaker)
            {
                fPageMaker = pageMaker;
            }

            #region IConfigCreator<IPageMaker> 成员

            public IPageMaker CreateObject(params object[] args)
            {
                return fPageMaker;
            }

            #endregion IConfigCreator<IPageMaker> 成员
        }

        [ObjectElement(NamespaceType.Toolkit)]
        public WebConfigItem WebConfig { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, LocalName = "ExceptionHandler")]
        protected ExceptionHandlerConfigItem InnerExceptionHandler { get; private set; }

        public IConfigCreator<IExceptionHandler> ExceptionHandler { get; private set; }

        public IConfigCreator<IExceptionHandler> ReLogOnHandler { get; private set; }

        public IConfigCreator<IExceptionHandler> ErrorPageHandler { get; private set; }

        public IConfigCreator<IExceptionHandler> ToolkitHandler { get; private set; }

        public IConfigCreator<IExceptionHandler> ErrorOperationHandler { get; private set; }

        public void OnReadObject()
        {
            var tempHandleConfig = new PageMakerExceptionHandlerConfig()
            {
                PageMaker = new TempPageMakerConfig(ExceptionPageMaker.Instance)
            };
            ExceptionHandler = InnerExceptionHandler?.Exception ??
                new PageMakerExceptionHandlerConfig
                {
                    Log = true,
                    PageMaker = new TempPageMakerConfig(ExceptionPageMaker.Instance)
                };
            ReLogOnHandler = InnerExceptionHandler?.ReLogonException ?? new ReLogonExceptionHandlerConfig();
            ErrorPageHandler = InnerExceptionHandler?.ErrorPageException ?? tempHandleConfig;
            ToolkitHandler = InnerExceptionHandler?.ToolkitException ?? tempHandleConfig;
            ErrorOperationHandler = InnerExceptionHandler?.ErrorOperationException ?? tempHandleConfig;
        }
    }
}