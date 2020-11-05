namespace YJC.Toolkit.SimpleWorkflow
{
    internal class AutoRNOState : InvalidState
    {
        public static readonly State Instance = new AutoRNOState();

        private AutoRNOState()
        {
        }

        public override string ToString()
        {
            return "自动步骤的未接收状态";
        }

        public override StepState StepState
        {
            get
            {
                return StepState.ReceiveNotOpen;
            }
        }
    }
}
