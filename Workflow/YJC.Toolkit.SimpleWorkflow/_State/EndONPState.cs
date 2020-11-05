namespace YJC.Toolkit.SimpleWorkflow
{
    internal class EndONPState : InvalidState
    {
        public static readonly State Instance = new EndONPState();

        private EndONPState()
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
            return "结束步骤的打开未处理状态";
        }
    }
}
