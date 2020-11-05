using System;
using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class EndStep : Step
    {
        public EndStep(Workflow workflow, StepConfig config)
            : base(workflow, config)
        {
        }

        protected override bool Execute()
        {
            string parentId = WorkflowRow["ParentId"].ToString();
            bool isChild = StepUtil.IsWorkflowType(WorkflowRow, WorkflowType.Child);
            EndStepConfig config = Config as EndStepConfig;
            AutoProcessor processor = null;
            WorkflowContent content = WorkflowInstResolver.CreateContent(WorkflowRow);
            using (content)
            {
                bool isFill = true;
                IEnumerable<TableResolver> tableResolvers = null;
                if (!string.IsNullOrEmpty(config.ProcessorConfig))
                {
                    try
                    {
                        processor = config.ProcessorConfig.CreateFromXmlFactoryUseJson<AutoProcessor>(
                            AutoProcessorConfigFactory.REG_NAME);
                        processor.Config = config;
                        processor.Source = Source;
                        processor.Content = content;
                        //填充Content
                        content.FillWithMainData(processor.FillMode, Source);
                        isFill = false;
                        //执行自动操作
                        tableResolvers = processor.Execute(WorkflowRow);
                    }
                    catch (Exception ex)
                    {
                        throw new PlugInException(config, config.Error, ex);
                    }
                }
                //else
                //    content.Fill(FillContentMode.MainOnly, Source);

                var finishData = StepUtil.FinishStep(Workflow, content, config, tableResolvers, processor, isFill);
                //notify发起人（如果有配置）
                config.Notify(content, finishData.WfHistoryRow, Source);
            }
            //notify父流程
            if (isChild)
            {
                StepUtil.NoifyParentStep(Workflow.Context, parentId);
            }
            return false;
        }

        protected override void Send(StepConfig nextStep)
        {
            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "工作流{1}的结束步骤{0}没有Send操作", Config.Parent.Name, Config.Name), this);
        }

        internal override State GetState(StepState state)
        {
            switch (state)
            {
                case StepState.NotReceive:
                    return EndNRState.Instance;

                case StepState.ReceiveNotOpen:
                    return EndRNOState.Instance;

                case StepState.OpenNotProcess:
                    return EndONPState.Instance;

                case StepState.ProcessNotSend:
                    return EndPNSState.Instance;

                case StepState.Mistake:
                    return EndMState.Instance;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }
    }
}