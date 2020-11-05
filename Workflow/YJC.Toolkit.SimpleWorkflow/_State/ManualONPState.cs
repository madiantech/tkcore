namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ManualONPState : State
    {
        public static readonly State Instance = new ManualONPState();

        private ManualONPState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.OpenNotProcess;
            }
        }

        public override bool Execute(Workflow workflow)
        {
            return false;
        }

        public override string ToString()
        {
            return "人工步骤的打开未处理状态";
        }
    }
}
