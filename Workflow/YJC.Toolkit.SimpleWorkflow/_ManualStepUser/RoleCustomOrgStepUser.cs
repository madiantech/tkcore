using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RoleCustomOrgStepUser : IManualStepUser
    {
        private readonly string fRoleCode;
        private readonly string fExpression;

        public RoleCustomOrgStepUser(string roleCode, string expression)
        {
            TkDebug.AssertArgumentNullOrEmpty(roleCode, "roleCode", null);
            TkDebug.AssertArgumentNullOrEmpty(expression, "expression", null);

            fRoleCode = roleCode;
            fExpression = expression;
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            var orgProvider = WorkflowSetting.Current.UserManager.OrgProvider;
            string orgCode = StepUtil.ExecuteExpression<string>(fExpression,
                 content.MainRow, workflowRow);
            var users = orgProvider.FindUsersByRole(orgCode, fRoleCode);
            return StepUserUtil.Convert(users);
        }

        #endregion IManualStepUser 成员
    }
}