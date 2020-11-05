using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public interface IManualStepUser
    {
        IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source);
    }
}