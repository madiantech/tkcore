using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [WorkflowStepConfig(RegName = "Auto")]
    internal class InternalAutoStepConfig : AutoStepConfig, IConfigCreator<StepConfig>
    {
        public InternalAutoStepConfig()
        {
        }

        internal InternalAutoStepConfig(WorkflowConfig workflowConfig)
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