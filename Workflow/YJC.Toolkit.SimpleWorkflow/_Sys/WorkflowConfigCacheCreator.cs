using System.Data;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2017-05-21", Description = "创建工作流配置")]
    [InstancePlugIn, ActiveTimeCache]
    internal class WorkflowConfigCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new WorkflowConfigCacheCreator();

        private WorkflowConfigCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            TkDbContext context = ObjectUtil.QueryObject<TkDbContext>(args);
            using (WorkflowSource source = new WorkflowSource(context, false))
            using (WorkflowDefResolver resolver = new WorkflowDefResolver(source))
            {
                DataRow row = resolver.SelectRowWithKeys(key);
                string xml = row["Content"].ToString();
                WorkflowConfig config = WorkflowConfig.ReadXml(xml);
                return config;
            }
        }
    }
}