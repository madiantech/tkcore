namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RouteNRState : NRState
    {
        public static readonly State Instance = new RouteNRState();

        private RouteNRState()
        {
        }

        public override string ToString()
        {
            return "路由步骤的未接收状态";
        }
    }
}
