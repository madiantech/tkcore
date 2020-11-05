namespace YJC.Toolkit.SimpleWorkflow
{
    internal abstract class InvalidState : State
    {
        protected InvalidState()
        {
        }

        public override bool Execute(Workflow workflow)
        {
            ThrowInvalidState();
            return false;
        }
    }
}
