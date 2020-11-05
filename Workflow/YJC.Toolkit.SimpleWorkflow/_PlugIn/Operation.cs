using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal abstract class Operation : BaseProcessor
    {
        private ManualStepConfig fConfig;

        protected Operation()
        {
        }

        public StepAction Action { get; protected set; }

        public string Message { get; protected set; }

        public ManualStepConfig ManualConfig
        {
            get
            {
                if (fConfig == null)
                {
                    fConfig = Config as ManualStepConfig;
                    TkDebug.AssertNotNull(fConfig, string.Format(ObjectUtil.SysCulture,
                        "当前步骤的Config必须是人工步骤的配置，现在的名称是{0}，类型是{1}",
                        Config.Name, Config.StepType), this);
                }
                return fConfig;
            }
        }

        public abstract IEnumerable<TableResolver> Execute(DataRow workflowRow, IParameter parameter);

        internal bool Execute(Workflow workflow, IParameter parameter, object userId)
        {
            DataRow workflowRow = workflow.WorkflowRow;
            IEnumerable<TableResolver> operateRes = Execute(workflowRow, parameter);

            switch (Action)
            {
                case StepAction.Abort:
                    workflow.Abort(userId.ToString(), operateRes, this);
                    break;

                case StepAction.Back:
                    ManualStep manualStep = workflow.CurrentStep as ManualStep;
                    SaveContent(workflowRow);
                    return manualStep.Back(userId.ToString(), Content, operateRes, this);
                //break;
                case StepAction.None:
                    bool isSave = SaveContent(workflowRow);
                    if (isSave || operateRes != null)
                    {
                        List<TableResolver> resolvers = new List<TableResolver>();
                        if (isSave)
                        {
                            workflow.WorkflowResolver.SetCommands(AdapterCommand.Update);
                            resolvers.Add(workflow.WorkflowResolver);
                        }
                        if (operateRes != null)
                            resolvers.AddRange(operateRes);
                        UpdateUtil.UpdateTableResolvers(Source.Context, ApplyDatas, resolvers);
                    }
                    break;

                case StepAction.Unlock:
                    ManualStep manualStep2 = workflow.CurrentStep as ManualStep;
                    SaveContent(workflowRow);
                    manualStep2.Unlock();
                    return false;

                case StepAction.Send:
                    WorkflowInstResolver.ManualSendWorkflow(workflowRow, userId, this);
                    workflow.WorkflowResolver.SetCommands(AdapterCommand.Update);
                    if (operateRes == null)
                        UpdateUtil.UpdateTableResolvers(Source.Context, ApplyDatas, workflow.WorkflowResolver);
                    else
                        UpdateUtil.UpdateTableResolvers(Source.Context, ApplyDatas, workflow.WorkflowResolver, operateRes);
                    workflow.UpdateState(ManualPNSState.Instance);
                    break;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return false;
            }
            return true;
        }
    }
}