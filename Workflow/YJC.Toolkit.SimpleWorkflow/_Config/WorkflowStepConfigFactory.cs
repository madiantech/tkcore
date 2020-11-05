using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class WorkflowStepConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_WorkflowStep";
        private const string DESCRIPTION = "WorkflowStep配置插件工厂";

        public WorkflowStepConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}