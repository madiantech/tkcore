using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WfProcessSource : BaseSimpleUpdateSource
    {
        private readonly ProcessXml fProcessXml;
        private bool fIsSave;
        private Processor fProcessor;
        private Workflow fWorkflow;
        private WorkflowContent fContent;

        private bool fIsEnableSave;
        //private int fApprove;
        //private string fApproveNote;
        //private bool fIsUserApprove;

        public WfProcessSource(ProcessXml processXml)
        {
            UseMetaData = true;
            fProcessXml = processXml;
            if (processXml.Relations != null)
            {
                foreach (var item in processXml.Relations)
                    AddRelation(item.MasterTableName, item.DetailTableName, item.CreateRelation());
            }
        }

        public DataRow WorkflowRow
        {
            get
            {
                if (fWorkflow != null)
                    return fWorkflow.WorkflowRow;
                return null;
            }
        }

        public WorkflowConfig Config
        {
            get
            {
                if (fWorkflow != null)
                    return fWorkflow.Config;
                return null;
            }
        }

        public StepConfig CurrentStepConfig
        {
            get
            {
                if (fWorkflow != null)
                    return fWorkflow.CurrentStep.Config;
                return null;
            }
        }

        private void CreateProcessor(IInputData input)
        {
            string workflowId = input.QueryString[WorkflowWebConst.QUERY_STRING_ID];
            fWorkflow = Workflow.CreateWorkflow(workflowId, this);
            ManualStepConfig config = fWorkflow.CurrentStep.Config as ManualStepConfig;
            TkDebug.AssertNotNull(config, "调用时机有误，当前的步骤必须是人工步骤，现在不是", this);
            fContent = WorkflowInstResolver.CreateContent(fWorkflow.WorkflowRow);

            string configPlugIn = config.Process.UIOperation.PlugIn;
            IConfigCreator<Processor> creator = configPlugIn.ReadJsonFromFactory(ProcessorConfigFactory.REG_NAME)
                .Convert<IConfigCreator<Processor>>();
            fProcessor = creator.CreateObject(fProcessXml);

            fProcessor.Config = config;
            fProcessor.Source = this;
            fProcessor.Content = fContent;
            //fIsUserApprove = config.UseApprove;
            fIsEnableSave = config.ContainsSave;

            fIsSave = !string.IsNullOrEmpty(input.QueryString["save"]);

            if (!fIsEnableSave && fIsSave && input.IsPost)
            {
                throw new ToolkitException("该步骤配置 未启用保存操作", this);
            }
            //if (input.IsPost && fIsUserApprove)
            //{
            //    DataSet postDataSet = input.PostObject.Convert<DataSet>();
            //    DataTable table = postDataSet.Tables["WF_APPROVE_TEMP"];
            //    if (table != null && table.Rows.Count > 0)
            //    {
            //        DataRow row = table.Rows[0];
            //        fApprove = row["Approve"].Value<int>();
            //        fApproveNote = row["Note"].ToString();
            //    }
            //}
        }

        //private void UpdateApproveWorkFlowRow()
        //{
        //    NonUIResolvers.Add(fWorkflow.WorkflowResolver);
        //    fWorkflow.WorkflowResolver.SetCommands(AdapterCommand.Update);
        //    WorkflowRow.BeginEdit();
        //    WorkflowRow["Approve"] = fApprove;
        //    WorkflowRow["ApproveNote"] = fApproveNote;
        //    WorkflowRow.EndEdit();
        //}

        //private void UpdateApprove()
        //{
        //    ApproveHistoryResolver approveRes = new ApproveHistoryResolver(this);
        //    DataRow approveRow = approveRes.NewRow();
        //    approveRow.BeginEdit();
        //    approveRow["Approve"] = fApprove;
        //    approveRow["Note"] = fApproveNote;

        // WorkflowRow.BeginEdit(); WorkflowRow["Approve"] = 1;//默认就是同意 WorkflowRow["ApproveNote"] =
        // string.Empty;//默认没有 意见 WorkflowRow.EndEdit();

        //    //approveRes.SetInsertRow(approveRow);
        //    approveRow.EndEdit();
        //    approveRes.SetCommands(AdapterCommand.Insert);
        //    NonUIResolvers.Add(approveRes);
        //}

        //private void AddHeadContent(ManualStepConfig manualStepConfig)
        //{
        //    DataTable table = DataSetUtil.CreateDataTable("WF_INFO", "WF_ID",
        //        "WF_WD_NAME", "WF_WI_NAME", "WF_STEP_NAME", "WF_IS_SAVE", "CONTENT_XML",
        //        "WF_APPROVE", "WF_APPROVE_NOTE", "WF_IS_APPROVE");

        // DataRow row = table.NewRow(); row.BeginEdit(); row["WF_WD_NAME"] =
        // fWorkflow.Config.DisplayName; row["WF_WI_NAME"] = fWorkflow.WorkflowRow["WI_NAME"];
        // row["WF_STEP_NAME"] = fWorkflow.CurrentStep.Config.DisplayName; //row["CONTENT_XML"] =
        // manualStepConfig.ContentXml; row["WF_ID"] = fWorkflow.WorkflowId;

        // row["WF_IS_SAVE"] = manualStepConfig.ContainsSave; row["WF_APPROVE"] =
        // fWorkflow.WorkflowRow["WI_APPROVE"] == DBNull.Value ? 1 :
        // fWorkflow.WorkflowRow["WI_APPROVE"]; row["WF_APPROVE_NOTE"] =
        // fWorkflow.WorkflowRow["WI_APPROVE_NOTE"]; row["WF_IS_APPROVE"] = fIsUserApprove;

        //    row.EndEdit();
        //    table.Rows.Add(row);
        //    DataSet.Tables.Add(table);
        //}

        protected override bool TestPageStyleForMetaData(IPageStyle style)
        {
            return true;
        }

        protected override IEnumerable<ResolverConfig> CreateUpdateTableResolvers(IInputData input)
        {
            var resolversConfigs = fProcessor.CreateUpdateResolverConfigs(WorkflowRow);
            if (fIsEnableSave)
            {
                foreach (var resolverConfig in resolversConfigs)
                {
                    if (resolverConfig.Kind == UpdateKind.Insert)
                    {
                        throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                            @"配置了 ""保存"" 按钮的步骤，process不可以配置insert操作的 ""{0}""",
                            resolverConfig.Resolver.ToString()), this);
                    }
                }
            }
            return resolversConfigs;
        }

        public override OutputData DoAction(IInputData input)
        {
            CreateProcessor(input);
            return base.DoAction(input);
        }

        protected override void ApplyDatas(Transaction transaction)
        {
            base.ApplyDatas(transaction);

            fProcessor.ApplyDatas(transaction);
        }

        protected override void OnCommittingData(CommittingDataEventArgs e)
        {
            base.OnCommittingData(e);

            if (!fIsSave)
            {
                if (fProcessor != null)
                {
                    fProcessor.OnCommittingData(e.InputData, WorkflowRow, fWorkflow.WorkflowResolver, fIsSave);
                    NonUIResolvers.Add(fWorkflow.WorkflowResolver);
                    if (fProcessor.Action == StepAction.Send)
                    {
                        WorkflowInstResolver.ManualSendWorkflow(WorkflowRow, BaseGlobalVariable.UserId, fProcessor);
                        fWorkflow.WorkflowResolver.SetCommands(AdapterCommand.Update);
                        fProcessor.OnSending(e.InputData, WorkflowRow, fWorkflow.WorkflowResolver);
                        //if (fIsUserApprove)
                        //{
                        //    UpdateApprove();
                        //}
                    }
                }
            }
            else
            {
                if (fProcessor != null)
                {
                    fProcessor.OnCommittingData(e.InputData, WorkflowRow, fWorkflow.WorkflowResolver, fIsSave);
                }
                //if (fIsUserApprove)
                //{
                //    UpdateApproveWorkFlowRow();
                //}
            }
        }

        protected override void OnCommittedData(CommittedDataEventArgs e)
        {
            base.OnCommittedData(e);

            //必须清空数据 为下次Fill 做准备
            if (fProcessor.Action == StepAction.Send && !fIsSave)
            {
                foreach (DataTable table in DataSet.Tables)
                {
                    if (table.TableName != "WF_WORKFLOW_INST")
                        table.Rows.Clear();
                }
                fWorkflow.UpdateState(ManualPNSState.Instance);
                fWorkflow.Run();
            }
        }

        protected override OutputData ReturnSucceedResult()
        {
            string url = WebUtil.ResolveUrl(fWorkflow.GetWorkflowUrl());
            return OutputData.CreateToolkitObject(new WebSuccessResult(url));
        }

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

        protected override void FillCustomTables(IInputData input)
        {
            fContent.Fill(fProcessor.FillMode, this);

            fProcessor.FillData(input, fWorkflow.WorkflowRow, fWorkflow.WorkflowResolver);
        }
    }
}