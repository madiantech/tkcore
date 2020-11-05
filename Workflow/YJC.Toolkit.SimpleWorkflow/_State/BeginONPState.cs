namespace YJC.Toolkit.SimpleWorkflow
{
    internal class BeginONPState : InvalidState
    {
        public static readonly State Instance = new BeginONPState();

        private BeginONPState()
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
            return "开始步骤的打开未处理状态";
        }
    }
}
