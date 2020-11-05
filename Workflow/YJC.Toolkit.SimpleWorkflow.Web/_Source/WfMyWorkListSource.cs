using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Source]
    internal class WfMyWorkListSource : DbListSource
    {
        private const int MAX_RECORD = 1000;

        public WfMyWorkListSource()
        {
            PageSize = MAX_RECORD;
            Context = WorkflowContext.CreateDbContext();
            MainResolver = new WorkflowInstWebResolver(this);
        }

        protected override IParamBuilder CreateCustomCondition(IInputData input)
        {
            return WorkflowUtil.CreateInsByStep(Context, WebGlobalVariable.UserId,
                input.QueryString["DefName"], input.QueryString["StepId"], input.QueryString["Name"]);
        }
    }
}