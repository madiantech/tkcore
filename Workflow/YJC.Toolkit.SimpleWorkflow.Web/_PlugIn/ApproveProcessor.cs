using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using System.Data;
using System.Collections.Specialized;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ApproveProcessor : XmlProcessor
    {
        private const string TEMP_APPROVE_TABLE_NAME = "_Approve_Temp";
        private const string APPROVE_FIELD = "Approve";
        private const string NOTE_FIELD = "ApproveNote";
        private static readonly string[] COPY_FIELDS = new string[] { APPROVE_FIELD, NOTE_FIELD };

        public ApproveProcessor(ProcessXml config)
            : base(config)
        {
            AssertFillMode(config);
        }

        private TableResolverCollection GetNonUIResolvers()
        {
            INonUIResolvers nonUIResolvers = Source as INonUIResolvers;
            TkDebug.AssertNotNull(nonUIResolvers, string.Format(ObjectUtil.SysCulture,
                "数据源必须实现INonUIResolvers接口，当前数据源类型是{0}，名称是{1}，没有实现", Source.GetType(), Source), Source);
            return nonUIResolvers.NonUIResolvers;
        }

        protected void AssertFillMode(ProcessXml config)
        {
            if (!(config.FillMode == FillContentMode.All || config
                    .FillMode == FillContentMode.MainOnly))
            {
                FillMode = FillContentMode.All;
            }
        }

        public override IEnumerable<ResolverConfig> CreateUpdateResolverConfigs(DataRow workflowRow)
        {
            if (XmlConfig == null || XmlConfig.TableList == null || XmlConfig.TableList.Count == 0)
                return Enumerable.Empty<ResolverConfig>();
            var tempResult = from item in XmlConfig.TableList
                             select item.CreateResolverConfig(Content, Source);
            var result = from item in tempResult
                         where item.Resolver.TableName != "WF_APPROVE_HISTORY"
                         select item;
            return result;
        }

        public override void FillData(IInputData input, DataRow workflowRow, TableResolver workflowResolver)
        {
            if (!input.IsPost)
            {
                DataTable table = DataSetUtil.CreateDataTable(TEMP_APPROVE_TABLE_NAME, APPROVE_FIELD, NOTE_FIELD);
                DataRow row = table.NewRow();
                DataSetUtil.CopyRow(workflowRow, row, COPY_FIELDS, COPY_FIELDS);
                if (row[APPROVE_FIELD] == DBNull.Value)
                    row[APPROVE_FIELD] = 1;
                table.Rows.Add(row);
                Source.DataSet.Tables.Add(table);

                using (ApproveHistoryResolver resolver = new ApproveHistoryResolver(Source))
                {
                    resolver.FillCodeTable((PageStyleClass)PageStyle.Update);
                }
            }
        }

        public override void OnCommittingData(IInputData input, DataRow workflowRow,
            TableResolver workflowResolver, bool isSave)
        {
            DataSet postDataSet = input.PostObject.Convert<DataSet>();
            DataTable table = postDataSet.Tables[TEMP_APPROVE_TABLE_NAME];
            TkDebug.Assert(table != null && table.Rows.Count == 1, "没有提交审批记录", this);

            DataRow postRow = table.Rows[0];
            DataSetUtil.CopyRow(postRow, workflowRow, COPY_FIELDS, COPY_FIELDS);
            workflowResolver.SetCommands(AdapterCommand.Update);
            GetNonUIResolvers().Add(workflowResolver);
        }

        public override void OnSending(IInputData input, DataRow workflowRow, TableResolver workflowResolver)
        {
            ApproveHistoryResolver approveRes = new ApproveHistoryResolver(Source);
            approveRes.SetCommands(AdapterCommand.Insert);
            DataRow approveRow = approveRes.NewRow();
            approveRow.BeginEdit();
            try
            {
                approveRow[APPROVE_FIELD] = workflowRow[APPROVE_FIELD];
                approveRow[NOTE_FIELD] = workflowRow[NOTE_FIELD];

                workflowRow.BeginEdit();
                try
                {
                    workflowRow["CustomData"] = workflowRow[APPROVE_FIELD];
                    workflowRow[APPROVE_FIELD] = 1;//默认就是同意
                    workflowRow[NOTE_FIELD] = string.Empty;//默认没有 意见
                }
                finally
                {
                    workflowRow.EndEdit();
                }

                approveRes.SetInsertRow(approveRow, Config, workflowRow["Id"].Value<int>());
            }
            finally
            {
                approveRow.EndEdit();
            }
            GetNonUIResolvers().Add(approveRes);
        }
    }
}