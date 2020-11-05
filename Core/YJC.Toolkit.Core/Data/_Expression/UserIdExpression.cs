using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Expression(SqlInject = true, Author = "YJC",
         CreateDate = "2013-12-31", Description = "登录用户的Id")]
    internal sealed class UserIdExpression : IExpression
    {
        internal const string REG_NAME = "UserId";

        #region IExpression 成员

        string IExpression.Execute()
        {
            TkDebug.ThrowIfNoGlobalVariable();

            object userId = BaseGlobalVariable.Current.UserInfo.UserId;
            return userId == null ? string.Empty : userId.ToString();
        }

        #endregion
    }
}
