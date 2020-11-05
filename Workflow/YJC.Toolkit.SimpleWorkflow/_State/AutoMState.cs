namespace YJC.Toolkit.SimpleWorkflow
{
    internal class AutoMState : MState
    {
        public static readonly State Instance = new AutoMState();

        private AutoMState()
        {
        }

        public override string ToString()
        {
            return "自动步骤的错误状态";
        }
    }
}
