using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ChildrenPartFinishMerger : IMerger
    {
        private const string SQL = "SELECT COUNT(*) FROM ";

        /// <summary>
        /// Initializes a new instance of the ChildrenPartFinishMerger class.
        /// </summary>
        public ChildrenPartFinishMerger(PartType type, double number)
        {
            Type = type;
            Number = number;
        }

        public PartType Type { get; private set; }

        public double Number { get; private set; }

        #region IMerger 成员

        public bool IsWait(int workflowId, DataRow workflowRow, DataRow mainRow, IDbDataSource source)
        {
            if (!StepUtil.IsWorkflowType(workflowRow, WorkflowType.Parent))
                return false;

            IParamBuilder builder = SqlParamBuilder.CreateEqualSql(source.Context,
                "WI_PARENT_ID", TkDataType.Int, workflowId);
            int liveCount = DbUtil.ExecuteScalar(SQL + "WF_WORKFLOW_INST",
                source.Context, builder).Value<int>();
            if (liveCount == 0)
                return false;
            int hisCount = DbUtil.ExecuteScalar(SQL + "WF_WORKFLOW_INST_HIS",
                source.Context, builder).Value<int>();
            switch (Type)
            {
                case PartType.Percent:
                    return (hisCount / (hisCount + liveCount)) < Number;

                case PartType.Count:
                    return hisCount < Convert.ToInt32(Number);
            }
            TkDebug.ThrowImpossibleCode(this);
            return true;
        }

        #endregion IMerger 成员

        public override string ToString()
        {
            if (Type == PartType.Percent)
                return string.Format(ObjectUtil.SysCulture, "等待{0}%的子流程完成的IMerger", Number);
            return string.Format(ObjectUtil.SysCulture, "等待{0}个子流程完成的IMerger",
                Convert.ToInt32(Number));
        }
    }
}