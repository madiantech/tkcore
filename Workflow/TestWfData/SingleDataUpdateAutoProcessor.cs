using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using YJC.Toolkit.SimpleWorkflow;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace ZyWorkOrder
{
    internal class SingleDataUpdateAutoProcessor : AutoProcessor
    {
        private string fDataXml;
        private List<KeyValueItem> fFieldList;

        public SingleDataUpdateAutoProcessor(SingleDataUpdateProcessorConfig config)
        {
            fDataXml = config.DataXml;
            fFieldList = config.FieldList;
            FillMode = FillContentMode.MainOnly;
        }

        public override IEnumerable<TableResolver> Execute(DataRow workflowRow)
        {
            DataRow row = Content.MainRow;
            row.BeginEdit();
            foreach (KeyValueItem field in fFieldList)
            {
                row[field.Key] = Expression.Execute(field.Value, Source);
            }
            row.EndEdit();
            Content.MainTableResolver.SetCommands(AdapterCommand.Update);
            return EnumUtil.Convert<TableResolver>(Content.MainTableResolver);
        }
    }
}