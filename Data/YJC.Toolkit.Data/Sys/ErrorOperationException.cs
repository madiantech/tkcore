using System;

namespace YJC.Toolkit.Sys
{
    public class ErrorOperationException : ToolkitException
    {
        protected ErrorOperationException()
        {

        }
        public ErrorOperationException(string message, object errorObject)
            : base(message, errorObject)
        {

        }
        public ErrorOperationException(string message, Exception innerException, object errorObject)
            : base(message, innerException, errorObject)
        {

        }
    }
}
