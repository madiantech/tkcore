using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [WorkflowStepConfig(RegName = "Begin")]
    internal class InternalBeginStepConfig : BeginStepConfig, IConfigCreator<StepConfig>
    {
        public InternalBeginStepConfig()
        {
        }

        internal InternalBeginStepConfig(WorkflowConfig workflowConfig)
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