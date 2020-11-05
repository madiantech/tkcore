using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class SingleUserStepUser : IManualStepUser
    {
        private readonly string fUserId;

        public SingleUserStepUser(string userId)
        {
            TkDebug.AssertArgumentNullOrEmpty(userId, "userId", null);

            fUserId = userId;
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            return EnumUtil.Convert(fUserId);
        }

        #endregion IManualStepUser 成员
    }
}