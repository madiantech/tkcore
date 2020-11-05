using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Source(Author = "Administrator", CreateDate = "2017-07-08",
        Description = "数据源")]
    internal class WfMyWorkSource : BaseDbSource
    {
        public WfMyWorkSource()
        {
            Context = WorkflowContext.CreateDbContext();
        }

        public override OutputData DoAction(IInputData input)
        {
            using (SqlSelector fSelector = new SqlSelector(this))
            {
                if (!input.IsPost)
                {
                    object userId = BaseGlobalVariable.UserId;
                    WorkflowUtil.AutoReceive(this, userId);
                    WorkflowUtil.SelectWorkflowStep(fSelector, userId, input);

                    string name = input.QueryString["Name"];
                    if (!string.IsNullOrEmpty(name))
                    {
                        DataTable dt = DataSetUtil.CreateDataTable("Params", "ParamName", "ParamValue");
                        DataRow dr = dt.NewRow();
                        dr["ParamName"] = "Name";
                        dr["ParamValue"] = name.Trim();
                        dt.Rows.Add(dr);
                        DataSet.Tables.Add(dt);
                    }
                    input.CallerInfo.AddInfo(DataSet);
                }
            }
            return OutputData.Create(DataSet);
        }
    }
}