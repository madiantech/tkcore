namespace YJC.Toolkit.SimpleWorkflow
{
    internal class BeginPNSState : PNSState
    {
        public static readonly State Instance = new BeginPNSState();

        private BeginPNSState()
        {
        }

        public override string ToString()
        {
            return "开始步骤的处理未发送状态";
        }
    }
}
