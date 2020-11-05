using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Resolver(Author = "YJC", CreateDate = "2017-12-31",
        Description = "StepInstHis 表的数据访问层类")]
    internal class StepInstHisWebResolver : Tk5TableResolver
    {
        private const string DATAXML = "Workflow/StepInst.xml";
        private const string TABLE_NAME = "WF_STEP_INST_HIS";

        public StepInstHisWebResolver(IDbDataSource source)
            : base(DATAXML, TABLE_NAME, source)
        {
        }
    }
}