using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class BeginStep : Step
    {
        public BeginStep(Workflow workflow, StepConfig config)
            : base(workflow, config)
        {
        }

        public IParameter Parameter { get; internal set; }

        protected override bool Execute()
        {
            BeginStepConfig config = Config as BeginStepConfig;
            using (WorkflowContent content = new WorkflowContent())
            {
                //创建creator 并执行creator相应方法
                Creator creator = config.CreatorConfig.CreateFromXmlFactoryUseJson<Creator>(
                    CreatorConfigFactory.REG_NAME);
                creator.Priority = config.Parent.Priority;
                creator.AddContent(Source, content, Parameter);
                if (Workflow.Config.UseApprove)
                {
                    content.AddContentItem(false, null, "WF_APPROVE_HISTORY",
                        "<tk:RegResolver>ApproveHistory</tk:RegResolver>".ReadXmlFromFactory
                        <IConfigCreator<TableResolver>>(ResolverCreatorConfigFactory.REG_NAME), null,
                        "WorkflowId", Workflow.WorkflowId.ToString(ObjectUtil.SysCulture));
                }
                content.FillWithMainData(creator.FillMode, Source);

                DataRow mainRow = content.MainRow;
                creator.SetWorkflowName(mainRow, Source);
                //更新新建的工作流实例信息
                DataRow row = WorkflowRow;
                row.BeginEdit();
                try
                {
                    row["Name"] = creator.WorkflowName;
                    row["ContentXml"] = content.CreateXml();
                    //步骤基本信息
                    WorkflowInstResolver.SetWorkflowByStep(row, config.Name, config.DisplayName, DateTime.Now,
                        (int)config.StepType, (int)StepState.ProcessNotSend);

                    row["Index"] = 1;
                    //row["WI_NEXT_INDEX"] = 2;
                    //优先级
                    row["Priority"] = (int)creator.Priority;
                    //扩展信息
                    if (!string.IsNullOrEmpty(Workflow.Config.OtherColumnOne))
                        WorkflowInstResolver.SetInstInfo(row, mainRow, Source.Context, "Info1",
                            Workflow.Config.OtherColumnOne);
                    if (!string.IsNullOrEmpty(Workflow.Config.OtherColumnTwo))
                        WorkflowInstResolver.SetInstInfo(row, mainRow, Source.Context, "Info2",
                            Workflow.Config.OtherColumnTwo);
                    //父子流程
                    int parentId = row["ParentId"].Value<int>();
                    if (parentId > 0)
                    {
                        DataRow parentRow = WorkflowResolver.SelectRowWithKeys(parentId);
                        parentRow["PcFlag"] = (int)(parentRow["PcFlag"].Value<WorkflowType>()
                            | WorkflowType.Parent);
                        WorkflowResolver.SetCommands(AdapterCommand.Update);
                    }
                }
                finally
                {
                    row.EndEdit();
                }

                TableResolver resolver = content.MainTableResolver;
                content.SetAllMainRow(config, config.Parent.MainTableColumnPrefix, row["Id"]);

                WorkflowResolver.SetCommands(AdapterCommand.Insert);
                resolver.SetCommands(AdapterCommand.Update);

                UpdateUtil.UpdateTableResolvers(Source.Context, null, WorkflowResolver, resolver);
            }
            return true;
        }

        protected override void Send(StepConfig nextStep)
        {
            StepUtil.SendStep(Workflow, nextStep);
        }

        internal override State GetState(StepState state)
        {
            switch (state)
            {
                case StepState.NotReceive:
                    return BeginNRState.Instance;

                case StepState.ReceiveNotOpen:
                    return BeginRNOState.Instance;

                case StepState.OpenNotProcess:
                    return BeginONPState.Instance;

                case StepState.ProcessNotSend:
                    return BeginPNSState.Instance;

                case StepState.Mistake:
                    return BeginMState.Instance;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }
    }
}