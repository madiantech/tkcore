
namespace YJC.Toolkit.Sys
{
    internal class ErrorPageSource : ISource
    {
        private readonly ErrorPageException fException;

        public ErrorPageSource(ErrorPageException exception)
        {
            fException = exception;
        }


        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            return OutputData.CreateObject(fException);
        }

        #endregion
    }
}
