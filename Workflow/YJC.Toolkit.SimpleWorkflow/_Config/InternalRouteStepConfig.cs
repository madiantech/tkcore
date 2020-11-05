using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [WorkflowStepConfig(RegName = "Route")]
    internal class InternalRouteStepConfig : RouteStepConfig, IConfigCreator<StepConfig>
    {
        public InternalRouteStepConfig()
        {
        }

        internal InternalRouteStepConfig(WorkflowConfig workflowConfig)
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