using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class OrgRoleStepUser : IManualStepUser
    {
        private readonly string fRoleCode;
        private readonly string fOrgCode;

        public OrgRoleStepUser(string roleCode, string orgCode)
        {
            TkDebug.AssertArgumentNullOrEmpty(roleCode, "roleCode", null);
            TkDebug.AssertArgumentNullOrEmpty(orgCode, "orgCode", null);

            fRoleCode = roleCode;
            fOrgCode = orgCode;
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            var orgProvider = WorkflowSetting.Current.UserManager.OrgProvider;
            var users = orgProvider.FindUsersByRole(fOrgCode, fRoleCode);

            return StepUserUtil.Convert(users);
        }

        #endregion IManualStepUser 成员
    }
}