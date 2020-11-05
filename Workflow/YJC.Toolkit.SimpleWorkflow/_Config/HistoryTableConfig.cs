using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class HistoryTableConfig
    {
        [SimpleAttribute]
        public string TableName { get; internal set; }

        [TagElement]
        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        public IConfigCreator<TableResolver> ResolverCreator { get; private set; }
    }
}