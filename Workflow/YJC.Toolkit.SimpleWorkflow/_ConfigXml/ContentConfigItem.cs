using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ContentConfigItem
    {
        [SimpleAttribute]
        public bool IsMain { get; private set; }

        [SimpleAttribute]
        public string OrderBy { get; private set; }

        [SimpleAttribute]
        public HistoryFillAction HistoryMethod { get; private set; }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME, Required = true)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> Resolver { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, Required = true, LocalName = "Record")]
        public List<RecordConfigItem> RecordList { get; private set; }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> HistoryResolver { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string FilterSql { get; private set; }
    }
}