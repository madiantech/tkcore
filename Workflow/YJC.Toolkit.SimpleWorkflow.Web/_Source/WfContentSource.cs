using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WfContentSource : BaseDbSource, ISupportMetaData
    {
        private readonly ContentXml fContentXml;
        private WorkflowContent fContent;
        private readonly IEnumerable<TableMetaDataConfig> fTables;
        private WorkflowMetaData fMetaData;
        private readonly bool fIsHistory;
        private readonly ProcessDisplay fDisplay;

        public WfContentSource(ContentXml config, ProcessDisplay display, bool isHistory)
        {
            Context = WorkflowContext.CreateDbContext();
            fContentXml = config;
            fTables = fContentXml.TableList;
            fIsHistory = isHistory;
            fDisplay = display;
        }

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData.Convert<WorkflowMetaData>();
        }

        #endregion ISupportMetaData 成员

        public override OutputData DoAction(IInputData input)
        {
            string id = input.QueryString[WorkflowWebConst.QUERY_STRING_ID];
            Tk5TableResolver stepResolver;
            Tk5TableResolver workflowRes;
            if (fIsHistory)
            {
                workflowRes = new WorkflowInstHisWebResolver(this);
                stepResolver = new StepInstHisWebResolver(this);
            }
            else
            {
                workflowRes = new WorkflowInstWebResolver(this);
                stepResolver = new StepInstWebResolver(this);
            }
            using (stepResolver)
            {
                using (workflowRes)
                {
                    fContent = WorkflowUtil.FillContent(this, stepResolver, id, workflowRes,
                        fDisplay, fIsHistory);
                }
                var tableResolvers = fContent.TableResolvers;
                foreach (var resolver in tableResolvers)
                {
                    var scheme = fMetaData.GetTableScheme(resolver.TableName);
                    if (scheme != null)
                        resolver.ReadMetaData(scheme);
                }

                if (!input.IsPost)
                {
                    var resolverStyle = from resolver in tableResolvers
                                        join tableData in fMetaData.Tables
                                        on resolver.TableName equals tableData.TableName
                                        select new { Resolver = resolver, Style = tableData.Style };
                    foreach (var item in resolverStyle)
                    {
                        MetaDataTableResolver metaResolver = item.Resolver as MetaDataTableResolver;
                        if (metaResolver != null)
                            metaResolver.Decode(item.Style);
                    }
                }
                //if (fIsHistory)
                //{
                //    stepResolver.HostTable.TableName = "WF_STEP_INST";
                //}
            }
            return OutputData.Create(DataSet);
        }
    }
}