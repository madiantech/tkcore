using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class CreatorStepUser : IManualStepUser
    {
        public static readonly IManualStepUser Instance = new CreatorStepUser();

        private CreatorStepUser()
        {
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            return EnumUtil.Convert(workflowRow["CreateUser"].ToString());
        }

        #endregion IManualStepUser 成员
    }
}