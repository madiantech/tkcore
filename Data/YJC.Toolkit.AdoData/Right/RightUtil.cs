using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    internal static class RightUtil
    {
        public static IDataRight SetErrorText(this IDataRight right, MultiLanguageText errorMessage)
        {
            if (right == null || errorMessage == null)
                return right;
            IRightCustomMessage message = right as IRightCustomMessage;
            if (message != null)
                message.ErrorMessage = errorMessage.ToString();

            return right;
        }
    }
}
