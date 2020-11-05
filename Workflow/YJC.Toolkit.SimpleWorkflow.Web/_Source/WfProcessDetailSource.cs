using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WfProcessDetailSource : BaseDbSource, ISupportMetaData
    {
        internal class ContentData
        {
            [SimpleAttribute]
            public string ContentUrl { get; set; }
        }

        private readonly ProcessXml fProcessXml;
        private WorkflowContent fContent;
        private readonly IEnumerable<TableMetaDataConfig> fTables;
        private WorkflowMetaData fMetaData;
        private Processor fProcessor;
        private Workflow fWorkflow;
        private ManualStepConfig fConfig;

        public WfProcessDetailSource(ProcessXml processXml)
        {
            Context = WorkflowContext.CreateDbContext();
            fProcessXml = processXml;
            fTables = fProcessXml.TableList;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fWorkflow.DisposeObject();
                fContent.DisposeObject();
                fProcessor.DisposeObject();
            }

            base.Dispose(disposing);
        }

        private void CreateProcessor(IInputData input, string workflowId)
        {
            fWorkflow = Workflow.CreateWorkflow(workflowId, this);
            fConfig = fWorkflow.CurrentStep.Config as ManualStepConfig;
            TkDebug.AssertNotNull(fConfig, "调用时机有误，当前的步骤必须是人工步骤，现在不是", this);
            fContent = WorkflowInstResolver.CreateContent(fWorkflow.WorkflowRow);

            string configPlugIn = fConfig.Process.UIOperation.PlugIn;
            IConfigCreator<Processor> creator = configPlugIn.ReadJsonFromFactory(ProcessorConfigFactory.REG_NAME)
                .Convert<IConfigCreator<Processor>>();
            fProcessor = creator.CreateObject(fProcessXml);

            fProcessor.Config = fConfig;
            fProcessor.Source = this;
            fProcessor.Content = fContent;
        }

        public override OutputData DoAction(IInputData input)
        {
            string id = input.QueryString[WorkflowWebConst.QUERY_STRING_ID];
            bool isHis = !string.IsNullOrEmpty(input.QueryString["HIS"]);
            CreateProcessor(input, id);

            Tk5TableResolver stepResolver;
            Tk5TableResolver workflowRes;
            if (isHis)
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
                    WorkflowUtil.FillContent(this, stepResolver, id, workflowRes, fContent,
                        ProcessDisplay.None, fWorkflow.WorkflowRow, false);
                }
                var tableResolvers = fContent.TableResolvers;
                foreach (var resolver in tableResolvers)
                {
                    var scheme = fMetaData.GetTableScheme(resolver.TableName);
                    if (scheme != null)
                        resolver.ReadMetaData(scheme);
                }
                FillDefaultValue();

                var resolverStyle = from resolver in tableResolvers
                                    join tableData in fMetaData.Tables
                                    on resolver.TableName equals tableData.TableName
                                    select new { Resolver = resolver, Style = tableData.Style };
                foreach (var item in resolverStyle)
                {
                    MetaDataTableResolver metaResolver = item.Resolver as MetaDataTableResolver;
                    if (metaResolver != null)
                    {
                        metaResolver.Decode(item.Style);
                        metaResolver.FillCodeTable((PageStyleClass)item.Style);
                    }
                }
                fProcessor.FillData(input, fWorkflow.WorkflowRow, fWorkflow.WorkflowResolver);
            }
            var contentData = new ContentData
            {
                ContentUrl = WebUtil.ResolveUrl(fWorkflow.GetContentUrl())
            };
            var tables = DataSet.Tables;
            tables.Add(EnumUtil.Convert(contentData).CreateTable("_Content_Data"));
            tables.Add(fConfig.Process.CreateTable(fConfig.ContainsSave));

            return OutputData.Create(DataSet);
        }

        private void FillDefaultValue()
        {
            var resolversConfigs = fProcessor.CreateUpdateResolverConfigs(fWorkflow.WorkflowRow);
            if (resolversConfigs != null)
            {
                var defaultConfigs = from item in resolversConfigs
                                     let pItem = item.Convert<ProcessResolverConfig>()
                                     where pItem.Config.DefaultValueList != null && pItem.Config.DefaultValueList.Count > 0
                                     select pItem;
                foreach (var config in defaultConfigs)
                {
                    DataTable table = config.Resolver.HostTable;
                    foreach (DataRow row in table.Rows)
                    {
                        row.BeginEdit();
                        try
                        {
                            foreach (var defaultConfig in config.Config.DefaultValueList)
                            {
                                if (defaultConfig.Force || (!defaultConfig.Force && row[defaultConfig.NickName] == DBNull.Value))
                                {
                                    if (string.IsNullOrEmpty(defaultConfig.Content))
                                    {
                                        row[defaultConfig.NickName] = DBNull.Value;
                                    }
                                    else
                                    {
                                        row[defaultConfig.NickName] = EvaluatorUtil.Execute(
                                            defaultConfig.Content, ("row", row));
                                    }
                                }
                            }
                        }
                        finally
                        {
                            row.EndEdit();
                        }
                    }
                }
            }
        }
    }
}