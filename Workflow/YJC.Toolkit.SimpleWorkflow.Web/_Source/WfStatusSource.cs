using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    [OutputRedirector]
    [Source(Author = "YJC", CreateDate = "2017-10-30",
        Description = "根据流程当前的状态，决定流程显示的页面")]
    internal class WfStatusSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            string wfId = input.QueryString[WorkflowWebConst.QUERY_STRING_ID];

            TkDbContext context = WorkflowContext.CreateDbContext();
            using (context)
            {
                Workflow workflow = Workflow.CreateWorkflow(context, wfId);
                using (workflow)
                {
                    string url = workflow.GetWorkflowUrl();
                    return OutputData.Create(url);
                }
            }
        }

        #endregion ISource 成员
    }
}