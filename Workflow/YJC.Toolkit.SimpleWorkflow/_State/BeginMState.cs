namespace YJC.Toolkit.SimpleWorkflow
{
    internal class BeginMState : MState
    {
        public static readonly State Instance = new BeginMState();

        private BeginMState()
        {
        }

        public override string ToString()
        {
            return "开始步骤的错误状态";
        }
    }
}
