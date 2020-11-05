using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Operation(Author = "YJC", CreateDate = "2018-04-09", Description = "流程反签收按钮的操作")]
    internal class UnlockOperation : Operation
    {
        public UnlockOperation()
        {
            Action = StepAction.Unlock;
        }

        public override IEnumerable<TableResolver> Execute(DataRow workflowRow, IParameter parameter)
        {
            return Enumerable.Empty<TableResolver>();
        }
    }
}