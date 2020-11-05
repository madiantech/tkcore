namespace YJC.Toolkit.SimpleWorkflow
{
    internal class EndRNOState : InvalidState
    {
        public static readonly State Instance = new EndRNOState();

        private EndRNOState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.ReceiveNotOpen;
            }
        }

        public override string ToString()
        {
            return "结束步骤的接收未打开状态";
        }
    }
}
