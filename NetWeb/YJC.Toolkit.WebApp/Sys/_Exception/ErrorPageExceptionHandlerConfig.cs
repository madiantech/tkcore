namespace YJC.Toolkit.Sys
{
    internal class ErrorPageExceptionHandlerConfig : IConfigCreator<IExceptionHandler>
    {
        #region IConfigCreator<IExceptionHandler> 成员

        public IExceptionHandler CreateObject(params object[] args)
        {
            //return ErrorPageExceptionHandler.Handler;
            return null;
        }

        #endregion
    }
}
