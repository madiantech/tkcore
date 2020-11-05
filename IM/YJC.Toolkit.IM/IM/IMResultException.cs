using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class IMResultException : ToolkitException
    {
        public IMResultException(BaseResult result)
            : this(result.ErrorCode, result.ErrorMsg)
        {
        }

        public IMResultException(int errorCode, string errorMessage)
            : base(errorMessage, null)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; private set; }
    }
}
