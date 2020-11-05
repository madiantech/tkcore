namespace YJC.Toolkit.Sys
{
    public class AssertException : ToolkitException
    {
        protected AssertException()
        {
        }

        public AssertException(string message, object errorObject)
            : base(message, errorObject)
        {
        }
    }
}
