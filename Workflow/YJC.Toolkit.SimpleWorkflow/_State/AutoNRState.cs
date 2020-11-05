namespace YJC.Toolkit.SimpleWorkflow
{
    internal class AutoNRState : NRState
    {
        public static readonly State Instance = new AutoNRState();

        private AutoNRState()
        {
        }

        public override string ToString()
        {
            return "自动步骤的未接收状态";
        }
    }

}
