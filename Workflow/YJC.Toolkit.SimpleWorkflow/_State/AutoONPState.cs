namespace YJC.Toolkit.SimpleWorkflow
{
    internal class AutoONPState : InvalidState
    {
        public static readonly State Instance = new AutoONPState();

        private AutoONPState()
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
            return "自动步骤的打开未处理状态";
        }
    }
}
