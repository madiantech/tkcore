using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [WorkflowStepConfig(RegName = "Manual")]
    internal class InternalManualStepConfig : ManualStepConfig, IConfigCreator<StepConfig>
    {
        public InternalManualStepConfig()
        {
        }

        internal InternalManualStepConfig(WorkflowConfig workflowConfig)
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