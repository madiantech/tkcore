namespace YJC.Toolkit.Sys
{
    [ExceptionHandlerConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-03",
        Author = "YJC", Description = "将页面重新定向到登录页，在Url中登记当前地址。这是默认的ReLogonException处理器")]
    internal class ReLogonExceptionHandlerConfig : IConfigCreator<IExceptionHandler>
    {
        #region IConfigCreator<IExceptionHandler> 成员

        public IExceptionHandler CreateObject(params object[] args)
        {
            return ReLogonExceptionHandler.Handler;
        }

        #endregion
    }
}
