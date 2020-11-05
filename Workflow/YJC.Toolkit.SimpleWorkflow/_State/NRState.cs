namespace YJC.Toolkit.SimpleWorkflow
{
    internal abstract class NRState : State
    {
        protected NRState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.NotReceive;
            }
        }

        public override bool Execute(Workflow workflow)
        {
            Step step = workflow.CurrentStep;
            bool result = step.ExecuteStep();
            State nextState = step.GetState(StepState.ProcessNotSend);
            workflow.UpdateState(nextState);
            return result;
        }
    }
}
