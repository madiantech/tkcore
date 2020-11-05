using System.Configuration;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class WorkflowContext
    {
        private static readonly WorkflowContext Current = new WorkflowContext();
        private readonly DbContextConfig fConfig;

        private WorkflowContext()
        {
            string configName = BaseGlobalVariable.Current.DefaultValue.GetSimpleDefaultValue("WorkflowDbContext");
            string contextName = string.IsNullOrEmpty(configName) ? "Workflow" : configName;
            fConfig = DbContextUtil.GetDbContextConfig(contextName);
        }

        private TkDbContext CreateContext()
        {
            return fConfig.CreateDbContext();
        }

        public static TkDbContext CreateDbContext()
        {
            return Current.CreateContext();
        }
    }
}