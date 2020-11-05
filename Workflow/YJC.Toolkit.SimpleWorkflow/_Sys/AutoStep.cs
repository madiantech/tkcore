using System;
using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class AutoStep : Step
    {
        public AutoStep(Workflow workflow, StepConfig config)
            : base(workflow, config)
        {
        }

        protected override bool Execute()
        {
            AutoStepConfig config = Config as AutoStepConfig;

            WorkflowContent content = WorkflowInstResolver.CreateContent(WorkflowRow);
            using (content)
            {
                AutoProcessor processor = config.ProcessorConfig.CreateFromXmlFactoryUseJson<AutoProcessor>(
                    AutoProcessorConfigFactory.REG_NAME);
                processor.Config = config;
                processor.Source = Source;
                processor.Content = content;

                //填充Content
                content.Fill(processor.FillMode, Source);
                IEnumerable<TableResolver> tableResolvers = null;
                try
                {
                    //执行自动操作
                    tableResolvers = processor.Execute(WorkflowRow);
                }
                catch (Exception ex)
                {
                    throw new PlugInException(config, config.Error, ex);
                }

                WorkflowRow.BeginEdit();
                try
                {
                    WorkflowRow["Status"] = (int)StepState.ProcessNotSend;
                    WorkflowInstResolver.ClearError(WorkflowRow);

                    processor.SaveContent(WorkflowRow);
                    if (processor.IsCreateSubWorkflow)
                    {
                        WorkflowRow["PcFlag"] = (int)(WorkflowRow["PcFlag"].Value<WorkflowType>()
                            | WorkflowType.Parent);
                    }
                    //AddContent
                }
                finally
                {
                    WorkflowRow.EndEdit();
                }
                WorkflowResolver.SetCommands(AdapterCommand.Update);
                //更新
                UpdateUtil.UpdateTableResolvers(Source.Context, processor.ApplyDatas,
                    WorkflowResolver, tableResolvers);
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
                    return AutoNRState.Instance;

                case StepState.ReceiveNotOpen:
                    return AutoRNOState.Instance;

                case StepState.OpenNotProcess:
                    return AutoONPState.Instance;

                case StepState.ProcessNotSend:
                    return AutoPNSState.Instance;

                case StepState.Mistake:
                    return AutoMState.Instance;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }
    }
}