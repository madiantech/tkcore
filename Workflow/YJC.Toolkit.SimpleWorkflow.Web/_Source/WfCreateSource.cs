using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Source(Author = "Administrator", CreateDate = "2017-05-26",
        Description = "创建工作流程的数据源")]
    internal class WfCreateSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            string name = input.QueryString["WFName"];
            TkDbContext context = WorkflowContext.CreateDbContext();
            using (context)
            {
                Workflow workflow = WorkflowUtil.CreateWorkflow(context, name,
                    Parameter.Create(input.QueryString),
                    BaseGlobalVariable.UserId.ToString());
                string retUrl = input.QueryString["RetURL1"];
                using (workflow)
                {
                    string workflowUrl = workflow.GetWorkflowUrl();
                    if (string.IsNullOrEmpty(retUrl))
                        retUrl = workflowUrl;
                    return OutputData.Create(retUrl);
                }
            }
        }

        #endregion ISource 成员
    }
}