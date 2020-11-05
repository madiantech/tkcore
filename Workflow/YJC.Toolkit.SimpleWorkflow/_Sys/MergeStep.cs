using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class MergeStep : Step
    {
        public MergeStep(Workflow workflow, StepConfig config)
            : base(workflow, config)
        {
        }

        protected override bool Execute()
        {
            //判断是否需要强制执行
            if (string.IsNullOrEmpty(WorkflowRow["AdminData"].ToString()))
            {
                MergeStepConfig config = Config as MergeStepConfig;
                IMerger merger = config.MergerConfig.CreateFromXmlFactoryUseJson<IMerger>(
                    MergerConfigFactory.REG_NAME);
                // 从配置中获取IMerger，计算后根据IMerger的结果返回
                try
                {
                    return !merger.IsWait(WorkflowRow["Id"].Value<int>(), WorkflowRow, null, Source);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                WorkflowRow["AdminData"] = DBNull.Value;
                return true;
            }
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
                    return MergeNRState.Instance;

                case StepState.ReceiveNotOpen:
                    return MergeRNOState.Instance;

                case StepState.OpenNotProcess:
                    return MergeONPState.Instance;

                case StepState.ProcessNotSend:
                    return MergePNSState.Instance;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }
    }
}