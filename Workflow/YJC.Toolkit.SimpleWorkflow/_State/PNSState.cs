using System.Linq;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal abstract class PNSState : State
    {
        protected PNSState()
        {
        }

        public sealed override StepState StepState
        {
            get
            {
                return StepState.ProcessNotSend;
            }
        }

        protected static bool SendStep(Workflow workflow, StepConfig nextConfig)
        {
            Step step = workflow.CurrentStep;
            bool result = step.SendStep(nextConfig);
            Step nextStep = nextConfig.CreateStep(workflow);
            workflow.UpdateStep(nextStep);
            State nextState = nextStep.GetState(StepState.NotReceive);
            workflow.UpdateState(nextState);
            return result;
        }

        public override bool Execute(Workflow workflow)
        {
            StepConfig nextConfig = workflow.CurrentStep.Config.NextSteps.First();
            return SendStep(workflow, nextConfig);
        }
    }
}
