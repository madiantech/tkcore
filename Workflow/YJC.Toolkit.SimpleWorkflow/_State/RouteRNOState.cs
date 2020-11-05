namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RouteRNOState : InvalidState
    {
        public static readonly State Instance = new RouteRNOState();

        private RouteRNOState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.ReceiveNotOpen;
            }
        }

        public override string ToString()
        {
            return "路由步骤的接收未打开状态";
        }
    }
}
