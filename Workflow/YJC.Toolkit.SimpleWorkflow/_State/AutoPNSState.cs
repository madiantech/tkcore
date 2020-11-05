namespace YJC.Toolkit.SimpleWorkflow
{
    internal class AutoPNSState : PNSState
    {
        public static readonly State Instance = new AutoPNSState();

        private AutoPNSState()
        {
        }

        public override string ToString()
        {
            return "自动步骤的处理未发送状态";
        }
    }
}
