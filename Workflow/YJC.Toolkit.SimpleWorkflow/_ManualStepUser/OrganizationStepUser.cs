using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class OrganizationStepUser : IManualStepUser
    {
        private readonly string fOrgCode;

        public OrganizationStepUser(string orgCode)
        {
            TkDebug.AssertArgumentNullOrEmpty(orgCode, "orgCode", null);

            fOrgCode = orgCode;
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            var orgProvider = WorkflowSetting.Current.UserManager.OrgProvider;
            var users = orgProvider.GetUsersInOrganization(fOrgCode);
            return StepUserUtil.Convert(users);
        }

        #endregion IManualStepUser 成员
    }
}