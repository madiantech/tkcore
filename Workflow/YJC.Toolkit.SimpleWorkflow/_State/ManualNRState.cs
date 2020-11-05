namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ManualNRState : State
    {
        public static readonly State Instance = new ManualNRState();

        private ManualNRState()
        {
        }

        public override bool Execute(Workflow workflow)
        {
            Step step = workflow.CurrentStep;
            bool result = step.ExecuteStep();
            return result;
        }

        public override string ToString()
        {
            return "人工步骤的未接收状态";
        }

        public override StepState StepState
        {
            get
            {
                return StepState.NotReceive;
            }
        }
    }
}