using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class MState : State
    {
        protected MState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.Mistake;
            }
        }

        public override bool Execute(Workflow workflow)
        {
            Step step = workflow.CurrentStep;
            StepState lastState;
            MistakeReason reason = workflow.WorkflowRow["ErrorType"].Value<MistakeReason>();
            if (reason == MistakeReason.NoActor)
                lastState = StepState.ProcessNotSend;
            else
                lastState = StepState.NotReceive;
            step.ClearDataSet();
            workflow.UpdateState(step.GetState(lastState));
            return true;
        }
    }
}