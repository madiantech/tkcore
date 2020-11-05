using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.SimpleWorkflow;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using System.Data;

namespace TestWfData
{
    [AutoProcessor]
    internal class TestAutoProcessor : AutoProcessor
    {
        public TestAutoProcessor()
        {
            FillMode = FillContentMode.MainOnly;
        }

        public override IEnumerable<TableResolver> Execute(DataRow workflowRow)
        {
            Tk5TableResolver resolver = new Tk5TableResolver(@"TestWf/Requirement.xml", Source);
            DataTable table = resolver.HostTable;
            DataRow row = table.Rows[0];
            row["FaqDate"] = DateTime.Today.AddDays(-10);
            resolver.SetCommands(AdapterCommand.Update);
            return EnumUtil.Convert<TableResolver>(resolver);
        }
    }
}