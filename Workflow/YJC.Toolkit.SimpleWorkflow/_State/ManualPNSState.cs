namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ManualPNSState : PNSState
    {
        public static readonly State Instance = new ManualPNSState();

        private ManualPNSState()
        {
        }

        public override string ToString()
        {
            return "人工步骤的处理未发送状态";
        }
    }
}
