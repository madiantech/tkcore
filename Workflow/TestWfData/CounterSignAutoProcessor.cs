using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.SimpleWorkflow;
using YJC.Toolkit.Sys;

namespace TestWfData
{
    internal class CounterSignAutoProcessor : AutoProcessor
    {
        private string fWfId;
        private readonly string fSubWorkflowName;
        private readonly string fUserConfig;

        public CounterSignAutoProcessor(string counterSignUserConfig, string subWorkflowName)
        {
            TkDebug.AssertArgumentNullOrEmpty(counterSignUserConfig, "counterSignUserConfig", null);
            TkDebug.AssertArgumentNullOrEmpty(subWorkflowName, "subWorkflowName", null);

            FillMode = FillContentMode.MainOnly;
            IsCreateSubWorkflow = true;
            fUserConfig = counterSignUserConfig;
            fSubWorkflowName = subWorkflowName;
        }

        public string TitleExpression { get; set; }

        public override IEnumerable<TableResolver> Execute(DataRow workflowRow)
        {
            IManualStepUser stepUser = fUserConfig.CreateFromXmlFactory<IManualStepUser>(
                ManualStepUserConfigFactory.REG_NAME);
            var users = stepUser.GetUserList(Content, workflowRow, Source);
            object wfId = workflowRow["Id"];
            using (CounterSignResolver resolver = new CounterSignResolver(Source))
            {
                resolver.SetCommands(AdapterCommand.Insert);
                string title;
                if (!string.IsNullOrEmpty(TitleExpression))
                    title = EvaluatorUtil.Execute<string>(TitleExpression,
                        ("dataRow", Content.MainRow),
                        ("workflowRow", workflowRow));
                else
                    title = null;

                foreach (var user in users)
                {
                    DataRow newRow = resolver.InsertRow(Content, workflowRow, user);
                    if (!string.IsNullOrEmpty(title))
                        newRow["WfTitle"] = title;
                }
                resolver.UpdateDatabase();

                var table = resolver.HostTable;
                foreach (DataRow row in table.Rows)
                {
                    IParameter parameter = Parameter.Create(new string[] { "Id", "MainId", "WfId" },
                        new string[] { row["Id"].ToString(), row["MainTableId"].ToString(), wfId.ToString() });
                    WorkflowUtil.CreateWorkflow(Source.Context, fSubWorkflowName, BaseGlobalVariable.UserId.ToString(),
                        wfId.Value<int>(), parameter);
                }
            }

            fWfId = wfId.ToString();
            return Enumerable.Empty<TableResolver>();
        }

        public override bool AddContent()
        {
            IConfigCreator<TableResolver> resolverCreator = "<tk:RegResolver>CounterSign</tk:RegResolver>"
                .ReadXmlFromFactory<IConfigCreator<TableResolver>>(ResolverCreatorConfigFactory.REG_NAME);
            Content.AddContentItem(null, "WF_COUNTER_SIGN", resolverCreator, null, "MainWfId", fWfId);
            return true;
        }
    }
}