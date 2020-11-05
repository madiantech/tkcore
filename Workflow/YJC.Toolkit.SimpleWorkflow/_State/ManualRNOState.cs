namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ManualRNOState : State
    {
        public static readonly State Instance = new ManualRNOState();

        private ManualRNOState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.ReceiveNotOpen;
            }
        }

        public override bool Execute(Workflow workflow)
        {
            return false;
        }

        public override string ToString()
        {
            return "人工步骤的接收未打开状态";
        }
    }
}
