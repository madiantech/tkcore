namespace YJC.Toolkit.Sys
{
    [ExceptionHandlerConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2019-09-27",
        Author = "YJC", Description = "直接抛错，使用.net core提供的ExceptionHandler处理错误")]
    internal class UseSystemExceptionHandlerConfig : IConfigCreator<IExceptionHandler>
    {
        public IExceptionHandler CreateObject(params object[] args)
        {
            return UseSystemExceptionHandler.Handler;
        }
    }
}