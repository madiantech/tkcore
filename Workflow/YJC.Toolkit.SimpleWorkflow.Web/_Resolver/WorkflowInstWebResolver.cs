using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Resolver(Author = "YJC", CreateDate = "2017-10-03",
        Description = "WorkflowInst 表的数据访问层类")]
    internal class WorkflowInstWebResolver : Tk5TableResolver
    {
        private const string DATAXML = "Workflow/WorkflowInst.xml";

        public WorkflowInstWebResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }
    }
}