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
    internal class CreateSubWfAutoProcessor : AutoProcessor
    {
        private const string USER_STR = "<tk:Composite><tk:Creator/><tk:SingleUser UserId='1001'/></tk:Composite>";

        public CreateSubWfAutoProcessor()
        {
            FillMode = FillContentMode.MainOnly;
        }

        public override IEnumerable<TableResolver> Execute(DataRow workflowRow)
        {
            IManualStepUser stepUser = USER_STR.CreateFromXmlFactory<IManualStepUser>(ManualStepUserConfigFactory.REG_NAME);
            var users = stepUser.GetUserList(Content, workflowRow, Source);
            using (CounterSignResolver resolver = new CounterSignResolver(Source))
            {
                resolver.SetCommands(AdapterCommand.Insert);
                foreach (var user in users)
                {
                    resolver.InsertRow(Content, workflowRow, user);
                }
                resolver.UpdateDatabase();

                var table = resolver.HostTable;
                foreach (DataRow row in table.Rows)
                {
                    IParameter parameter = Parameter.Create(new string[] { "Id", "MainId" },
                        new string[] { row["Id"].ToString(), row["MainTableId"].ToString() });
                    WorkflowUtil.CreateWorkflow(Source.Context, "CounterSign", "1", 40, parameter);
                }
            }
            return Enumerable.Empty<TableResolver>();
        }
    }
}