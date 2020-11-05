namespace YJC.Toolkit.SimpleWorkflow
{
    internal class EndPNSState : InvalidState
    {
        public static readonly State Instance = new EndPNSState();

        private EndPNSState()
        {
        }

        public override StepState StepState
        {
            get
            {
                return StepState.ProcessNotSend;
            }
        }

        public override string ToString()
        {
            return "结束步骤的处理未发送状态";
        }
    }
}
