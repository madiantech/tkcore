using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    /// <summary>
    /// 表审批历史表(WF_APPROVE_HISTORY)的数据访问层类
    /// </summary>
    [Resolver(Description = "表审批历史表(WF_APPROVE_HISTORY)的数据访问层类",
        Author = "YJC", CreateDate = "2018-04-03")]
    public class ApproveHistoryResolver : Tk5TableResolver
    {
        internal const string DATAXML = "workflow/ApproveHistory.xml";

        public ApproveHistoryResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }

        public void SetInsertRow(DataRow row, StepConfig step, int workflowId)
        {
            //IWorkflowConfig config = Source as IWorkflowConfig;
            //TkDebug.AssertNotNull(config, string.Format(ObjectUtil.SysCulture,
            //    "Resolver {0}的SOURCE应该实现IWorkflowConfig接口", REG_NAME), this);
            //StepConfig step = config.CurrentStepConfig;
            ////设置审批结果
            //config.WorkflowRow["CustomData"] = row["Approve"];
            row["WorkflowId"] = workflowId;
            row["StepName"] = step.Name;
            row["StepDisplayName"] = step.DisplayName;
            row["Id"] = CreateUniId();
            row["Operator"] = BaseGlobalVariable.UserId;
            row["CreateId"] = BaseGlobalVariable.UserId;
            row["CreateDate"] = DateTime.Now;
        }
    }
}