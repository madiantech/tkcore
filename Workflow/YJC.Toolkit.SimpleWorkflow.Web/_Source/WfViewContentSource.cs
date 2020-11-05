using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    [OutputRedirector]
    [Source(Author = "YJC", CreateDate = "2018-04-20",
        Description = "定向查看Content的Source")]
    internal class WfViewContentSource : ISource
    {
        public OutputData DoAction(IInputData input)
        {
            bool isHis = input.QueryString["view"] == "history";
            string wfId = input.QueryString[WorkflowWebConst.QUERY_STRING_ID];
            TkDbContext context = WorkflowContext.CreateDbContext();
            using (context)
            {
                if (isHis)
                {
                    string url = WorkflowExtension.GetHistoryContentUrl(context, wfId);
                    return OutputData.Create(url);
                }
                else
                {
                    Workflow workflow = Workflow.CreateWorkflow(context, wfId);
                    using (workflow)
                    {
                        string url = workflow.GetContentUrl(true);
                        return OutputData.Create(url);
                    }
                }
            }
        }
    }
}