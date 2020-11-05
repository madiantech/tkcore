using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Resolver(Author = "YJC", CreateDate = "2018-03-27",
        Description = "WorkflowInst 表的数据访问层类")]
    internal class WorkflowInstHisWebResolver : Tk5TableResolver
    {
        private const string DATAXML = "Workflow/WorkflowInstHis.xml";

        public WorkflowInstHisWebResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }
    }
}