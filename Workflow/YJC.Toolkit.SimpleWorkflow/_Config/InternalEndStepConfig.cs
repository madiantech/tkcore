using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [WorkflowStepConfig(RegName = "End")]
    internal class InternalEndStepConfig : EndStepConfig, IConfigCreator<StepConfig>
    {
        public InternalEndStepConfig()
        {
        }

        internal InternalEndStepConfig(WorkflowConfig workflowConfig)
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