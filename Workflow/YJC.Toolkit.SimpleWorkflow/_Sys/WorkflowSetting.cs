using YJC.Toolkit.Right;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class WorkflowSetting
    {
        private WorkflowSetting(WorkflowConfigXml xml)
        {
            UserManager = xml.UserManager.CreateObject();
        }

        public IUserManager UserManager { get; private set; }

        public static WorkflowSetting Current { get; private set; }

        internal static WorkflowSetting CreateWorkflowSetting(WorkflowConfigXml xml)
        {
            Current = new WorkflowSetting(xml);
            return Current;
        }
    }
}