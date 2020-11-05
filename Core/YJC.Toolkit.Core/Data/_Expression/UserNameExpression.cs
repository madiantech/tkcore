using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Expression(SqlInject = true, Author = "YJC",
         CreateDate = "2013-12-31", Description = "登录用户的Name")]
    internal sealed class UserNameExpression : IExpression
    {
        internal const string REG_NAME = "UserName";

        #region IExpression 成员

        string IExpression.Execute()
        {
            TkDebug.ThrowIfNoGlobalVariable();

            return BaseGlobalVariable.Current.UserInfo.UserName ?? string.Empty;
        }

        #endregion
    }
}
