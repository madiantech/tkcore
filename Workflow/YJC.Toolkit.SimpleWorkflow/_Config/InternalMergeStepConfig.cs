using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [WorkflowStepConfig(RegName = "Merge")]
    internal sealed class InternalMergeStepConfig : MergeStepConfig, IConfigCreator<StepConfig>
    {
        public InternalMergeStepConfig()
        {
        }

        internal InternalMergeStepConfig(WorkflowConfig workflowConfig)
            : base(workflowConfig)
        {
        }

        #region IConfigCreator<StepConfig> 成员

        public StepConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<StepConfig> 成员
    }
}