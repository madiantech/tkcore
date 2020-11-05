namespace YJC.Toolkit.SimpleWorkflow
{
    internal class MergeNRState : NRState
    {
        public static readonly State Instance = new MergeNRState();

        private MergeNRState()
        {
        }

        public override bool Execute(Workflow workflow)
        {
            Step step = workflow.CurrentStep;
            bool result = step.ExecuteStep();
            if (result)
            {
                State nextState = step.GetState(StepState.ProcessNotSend);
                workflow.UpdateState(nextState);
            }
            else
            {
                workflow.UpdateProcessDate();
            }
            return result;
        }

        public override string ToString()
        {
            return "聚合步骤的未接收状态";
        }
    }
}