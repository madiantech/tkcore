using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ChildrenFinishMerger : IMerger
    {
        private const string SQL = "SELECT COUNT(*) FROM WF_WORKFLOW_INST";

        #region IMerger 成员

        public bool IsWait(int workflowId, DataRow workflowRow, DataRow mainRow, IDbDataSource source)
        {
            if (!StepUtil.IsWorkflowType(workflowRow, WorkflowType.Parent))
                return false;

            IParamBuilder builder = SqlParamBuilder.CreateEqualSql(source.Context,
                "WI_PARENT_ID", TkDataType.Int, workflowId);
            int count = DbUtil.ExecuteScalar(SQL, source.Context, builder).Value<int>();
            return count > 0;
        }

        #endregion IMerger 成员

        public override string ToString()
        {
            return "等待所有子流程完成的IMerger";
        }
    }
}