namespace YJC.Toolkit.SimpleWorkflow
{
    internal class MergePNSState : PNSState
    {
        public static readonly State Instance = new MergePNSState();

        private MergePNSState()
        {
        }

        public override string ToString()
        {
            return "聚合步骤的处理未发送状态";
        }
    }
}
