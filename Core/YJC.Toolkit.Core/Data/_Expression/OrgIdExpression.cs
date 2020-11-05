using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Expression(SqlInject = true, Author = "YJC",
         CreateDate = "2013-12-31", Description = "登陆用户的机构Id")]
    internal sealed class OrgIdExpression : IExpression
    {
        internal const string REG_NAME = "OrgId";

        #region IExpression 成员

        string IExpression.Execute()
        {
            TkDebug.ThrowIfNoGlobalVariable();

            object orgId = BaseGlobalVariable.Current.UserInfo.MainOrgId;
            return orgId == null ? string.Empty : orgId.ToString();
        }

        #endregion
    }
}
