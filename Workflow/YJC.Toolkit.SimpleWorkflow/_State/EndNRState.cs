namespace YJC.Toolkit.SimpleWorkflow
{
    internal class EndNRState : NRState
    {
        public static readonly State Instance = new EndNRState();

        private EndNRState()
        {
        }

        public override bool Execute(Workflow workflow)
        {
            bool result = base.Execute(workflow);
            workflow.IsFinish = true;
            return result;
        }

        public override string ToString()
        {
            return "结束步骤的未接收状态";
        }
    }
}
