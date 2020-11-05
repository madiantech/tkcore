namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RoutePNSState : PNSState
    {
        public static readonly State Instance = new RoutePNSState();

        private RoutePNSState()
        {
        }

        public override bool Execute(Workflow workflow)
        {
            string nextStepName = workflow.WorkflowRow["CustomData"].ToString();
            //workflow.WorkflowRow["WI_CUSTOM_DATA"] = DBNull.Value;
            StepConfig config = workflow.Config.Steps[nextStepName];
            return SendStep(workflow, config);
        }

        public override string ToString()
        {
            return "路由步骤的处理未发送状态";
        }
    }
}