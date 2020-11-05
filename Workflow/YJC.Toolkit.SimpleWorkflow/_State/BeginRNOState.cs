namespace YJC.Toolkit.SimpleWorkflow
{
    internal class BeginRNOState : InvalidState
    {
        public static readonly State Instance = new BeginRNOState();

        private BeginRNOState()
        {
        }

        public override string ToString()
        {
            return "开始步骤的未接收状态";
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
