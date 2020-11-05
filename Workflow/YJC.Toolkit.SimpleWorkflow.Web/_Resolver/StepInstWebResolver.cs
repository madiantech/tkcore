using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Resolver(Author = "YJC", CreateDate = "2017-12-31",
        Description = "(StepInst)的数据访问层类")]
    internal class StepInstWebResolver : Tk5TableResolver
    {
        private const string DATAXML = "WorkFlow/StepInst.xml";

        public StepInstWebResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
            AutoTrackField = true;
            AutoUpdateKey = true;
        }
    }
}