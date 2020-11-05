namespace YJC.Toolkit.SimpleWorkflow
{
    internal class MergeRNOState : InvalidState
    {
        public static readonly State Instance = new MergeRNOState();

        private MergeRNOState()
        {
        }

        public override string ToString()
        {
            return "聚合步骤的未接收状态";
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
