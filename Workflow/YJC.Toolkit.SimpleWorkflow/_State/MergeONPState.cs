namespace YJC.Toolkit.SimpleWorkflow
{
    internal class MergeONPState : InvalidState
    {
        public static readonly State Instance = new MergeONPState();

        private MergeONPState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.OpenNotProcess;
            }
        }

        public override string ToString()
        {
            return "聚合步骤的打开未处理状态";
        }
    }
}
