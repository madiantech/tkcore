using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ExceptionHandlerConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2015-08-21",
        Author = "YJC", Description = "使用Razor的方式处理ErrorPageException的Exception处理器")]
    internal class RazorErrorPageExceptionHandlerConfig : IConfigCreator<IExceptionHandler>
    {
        #region IConfigCreator<IExceptionHandler> 成员

        public IExceptionHandler CreateObject(params object[] args)
        {
            return new RazorErrorPageExceptionHandler();
        }

        #endregion
    }
}
