namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RouteONPState : InvalidState
    {
        public static readonly State Instance = new RouteONPState();

        private RouteONPState()
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
            return "路由步骤的打开未处理状态";
        }
    }
}
