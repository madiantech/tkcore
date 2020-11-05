using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Operation(Author = "YJC", CreateDate = "2018-04-09", Description = "流程终止按钮的操作")]
    internal sealed class AbortOperation : Operation
    {
        public AbortOperation()
        {
            Action = StepAction.Abort;
        }

        public override IEnumerable<TableResolver> Execute(DataRow workflowRow, IParameter parameter)
        {
            return Enumerable.Empty<TableResolver>();
        }
    }
}