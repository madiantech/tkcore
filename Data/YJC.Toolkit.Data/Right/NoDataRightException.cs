using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class NoDataRightException : ToolkitException
    {
        public NoDataRightException()
            : this(null)
        {
        }

        public NoDataRightException(string message)
            : base(GetMessage(message), null)
        {
        }

        private static string GetMessage(string message)
        {
            return string.IsNullOrEmpty(message) ? "您没有权限操作此记录。" : message;
        }
    }
}
