using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ConnectionPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_Workflow_Connection";
        private const string DESCRIPTION = "Connection插件工厂";

        public ConnectionPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}