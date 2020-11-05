using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal static class StepUtil
    {
        private static readonly string[] COLUMN_NAMES = new string[] { "Id",
            "WdName", "ContentXml", "Name", "CreateDate", "CreateUser",
            "RefList", "IsTimeout", "TimeoutDate", "NextExeDate",  "Priority",
            "Status", "ErrorType", "RetryTimes", "MaxRetryTimes", "WeId",
            "ParentId", "PcFlag"};

        private const string UPDATE_INDEX_SQL = " UPDATE WF_STEP_INST SET SI_VALID_FLAG = {0} WHERE  SI_WI_ID = {1}"
            + " AND  SI_INDEX >= {2} AND SI_VALID_FLAG = {3}";

        private const string WHERE_INDEX_SQL = " WHERE {0} = {1} AND {2} = {3} "
            + "  AND {4} = {5} ";

        public static void CopyWorkflowToStep(TkDbContext context, DataRow workflowRow,
            DataRow stepRow, DateTime now, FlowAction flowAction)
        {
            stepRow["Index"] = workflowRow["Index"];
            //步骤标识 0 -- 有效初始值  1 -- 无效的
            stepRow["ValidFlag"] = 0;
            //流转方式
            stepRow["FlowType"] = flowAction;
            //步骤基本信息
            stepRow["Id"] = context.GetUniId(StepInstResolver.TABLE_NAME);
            stepRow["WiId"] = workflowRow["Id"];
            //上一步骤信息
            stepRow["LastStep"] = workflowRow["LastStep"];
            stepRow["LastStepName"] = workflowRow["LastStepName"];
            stepRow["LastManual"] = workflowRow["LastManual"];
            stepRow["LastManualName"] = workflowRow["LastManualName"];
            stepRow["LastDisplayName"] = workflowRow["LastDisplayName"];
            //当前步骤
            stepRow["CurrentStep"] = workflowRow["CurrentStep"];
            stepRow["CurrentStepName"] = workflowRow["CurrentStepName"];
            stepRow["StepType"] = workflowRow["StepType"];
            stepRow["Priority"] = workflowRow["Priority"];
            stepRow["Status"] = workflowRow["Status"];
            //当前步骤 时间信息：开始时间 结束时间 步骤用时
            stepRow["StartDate"] = workflowRow["CurrentCreateDate"];
            stepRow["EndDate"] = now;
            stepRow["TimeSpan"] = (double)((now - stepRow["StartDate"].Value<DateTime>()).Ticks)
               / TimeSpan.TicksPerDay;
            //发送时间
            stepRow["SendDate"] = workflowRow["SendDate"];
        }

        public static void CopyManualInfo(DataRow workflowRow, DataRow stepRow)
        {
            //当前步骤是否超时 超时时间 (超时信息)
            stepRow["IsTimeout"] = workflowRow["IsTimeout"];
            stepRow["TimeoutDate"] = workflowRow["TimeoutDate"];
            //接收 发送 处理人和时间（操作信息）
            stepRow["ReceiveId"] = workflowRow["ReceiveId"];
            stepRow["ReceiveDate"] = workflowRow["ReceiveDate"];

            stepRow["SendId"] = workflowRow["SendId"];
            stepRow["SendDate"] = workflowRow["SendDate"];

            stepRow["ProcessId"] = workflowRow["ProcessId"];
            stepRow["ProcessDate"] = workflowRow["ProcessDate"];
        }

        private static void CopyDate(DataRow workflowRow, DataRow stepRow)
        {
            // 非人工步骤，接收时间可以认为就是上一步的发送时间
            stepRow["ReceiveDate"] = workflowRow["SendDate"];
            stepRow["SendDate"] = workflowRow["SendDate"];
            stepRow["ProcessDate"] = workflowRow["ProcessDate"];
        }

        public static void SetWorkflowByStep(WorkflowContent content, IDbDataSource source,
            DataRow workflowRow, StepConfig nextStep)
        {
            workflowRow["LastStep"] = workflowRow["CurrentStep"];
            workflowRow["LastStepName"] = workflowRow["CurrentStepName"];

            WorkflowInstResolver.SetWorkflowByStep(workflowRow, nextStep);
            nextStep.Prepare(content, workflowRow, source);
        }

        internal static void SendStep(Workflow workflow, StepConfig nextStep)
        {
            DateTime now = DateTime.Now;
            DataRow workflowRow = workflow.WorkflowRow;
            StepInstResolver stepResolver = workflow.StepResolver;
            WorkflowInstResolver workflowResolver = workflow.WorkflowResolver;
            IDbDataSource source = workflow.Source;

            WorkflowContent content = WorkflowInstResolver.CreateContent(workflowRow);
            using (content)
            {
                //新步骤拷贝
                DataRow stepRow = stepResolver.NewRow();
                bool isManual = false;
                StepType stepType = StepType.None;

                stepRow.BeginEdit();
                try
                {
                    CopyWorkflowToStep(source.Context, workflowRow, stepRow, now, FlowAction.Flow);

                    stepType = workflow.CurrentStep.Config.StepType;
                    isManual = stepType == StepType.Manual;
                    if (isManual)
                    {
                        CopyManualInfo(workflowRow, stepRow);
                    }
                    else
                        CopyDate(workflowRow, stepRow);
                }
                finally
                {
                    stepRow.EndEdit();
                }

                //更新工作流实例
                workflowRow.BeginEdit();
                try
                {
                    workflowRow["Index"] = workflowRow["Index"].Value<int>() + 1;

                    if (isManual)
                    {
                        workflowRow["LastManual"] = workflowRow["CurrentStep"];
                        workflowRow["LastManualName"] = workflowRow["CurrentStepName"];
                        workflowRow["LastDisplayName"] = workflowRow["CurrentStepName"];
                        //更新参与人列表
                        string refIds = workflowRow["RefList"].ToString();
                        QuoteStringList ulRef = (QuoteStringList)refIds;
                        string receiveId = workflowRow["ReceiveId"].ToString();
                        string sendId = workflowRow["SendId"].ToString();
                        string processId = workflowRow["ProcessId"].ToString();
                        ulRef.Add(receiveId);
                        ulRef.Add(sendId);
                        ulRef.Add(processId);
                        workflowRow["RefList"] = ulRef.ToString();
                        //接收人  处理人  重新置为空
                        workflowRow["ReceiveId"] = DBNull.Value;
                        workflowRow["LastProcessId"] = workflowRow["ProcessId"];
                        workflowRow["ProcessId"] = DBNull.Value;
                        //清空超时和提醒标识
                        if (workflowRow["IsTimeout"].Value<bool>())
                        {
                            workflowRow["IsTimeout"] = false;
                        }
                        //if (workflowRow["WI_IS_NOTIFY"].Value<bool>() == true)
                        //{
                        //    workflowRow["WI_IS_NOTIFY"] = false;
                        //}
                        //清空错误处理信息 WI_ERROR_TYPE WI_MAX_RETRY_TIMES WI_RETRY_TIMES  WI_NEXT_EXE_DATE
                        WorkflowInstResolver.ClearError(workflowRow);
                    }
                    if ((stepType == StepType.Begin && workflowRow["PcFlag"].Value<WorkflowType>() != WorkflowType.Child)
                        || stepType == StepType.Merge)
                    {
                        workflowRow["LastDisplayName"] = workflowRow["CurrentStepName"];
                    }
                    //更新主表信息
                    content.Fill(FillContentMode.MainOnly, source);
                    SetWorkflowByStep(content, source, workflowRow, nextStep);
                }
                finally
                {
                    workflowRow.EndEdit();
                }

                TableResolver resolver = content.MainTableResolver;
                content.SetMainRowStatus(nextStep, workflow.Config.MainTableColumnPrefix);

                //更新
                workflowResolver.SetCommands(AdapterCommand.Update);
                stepResolver.SetCommands(AdapterCommand.Insert);
                resolver.SetCommands(AdapterCommand.Update);

                UpdateUtil.UpdateTableResolvers(source.Context, null, workflowResolver,
                    stepResolver, resolver);
            }
        }

        public static bool IsWorkflowType(DataRow workflowRow, WorkflowType type)
        {
            object value = workflowRow["PcFlag"];
            if (value == DBNull.Value)
                return false;
            if ((value.Value<WorkflowType>() & type) != type)
                return false;
            return true;
        }

        #region EndStep

        public static void ErrorAbort(Workflow workflow, FinishType finishType)
        {
            using (FinishData finishData = AbortWorkflow(workflow, finishType, null))
            {
                finishData.Resolvers.Commit();
            }
        }

        public static void Abort(Workflow workflow, object userId)
        {
            using (FinishData finishData = AbortWorkflow(workflow, FinishType.Abort, userId))
            {
                finishData.Resolvers.Commit();
            }
        }

        public static void Abort(Workflow workflow, object userId,
            IEnumerable<TableResolver> tableResolvers, BaseProcessor processor)
        {
            using (FinishData finishData = AbortWorkflow(workflow, FinishType.Abort, userId))
            {
                finishData.Resolvers.ApplyData += processor.ApplyDatas;
                processor.SaveContent(finishData.WfHistoryRow);
                finishData.Resolvers.AddResolvers(tableResolvers);

                finishData.Resolvers.Commit();
            }
        }

        public static bool BackFinish(Workflow workflow, WorkflowContent content, object userId,
            IEnumerable<TableResolver> tableResolvers, StepConfig nextStep, BaseProcessor processor,
            AutoProcessor autoProcessor)
        {
            bool isEnd = false;
            FinishData result = null;
            if (nextStep is BeginStepConfig)
            {
                result = FinishContentStep(workflow, FinishType.ReturnBegin, userId, content, true);
            }
            else
            {
                if (nextStep is ManualStepConfig)
                {
                    isEnd = true;
                    // TkDebug.Assert(nextStep is ManualStepConfig, string.Format(ObjectUtil.SysCulture,
                    //"步骤{0}不是人工步骤和开始步骤 无法退回", nextStep.Name), workflow);
                    result = BackStep(workflow, content, userId, nextStep, processor, autoProcessor);
                }
                else
                {
                    throw new ToolkitException(string.Format(ObjectUtil.SysCulture, "步骤{0}不是人工步骤或开始步骤 无法退回",
                        nextStep.Name), workflow);
                }
            }
            result.Content = content;
            using (result)
            {
                result.Resolvers.AddResolvers(tableResolvers);

                result.Resolvers.Commit();
            }
            return isEnd;
        }

        private static FinishData BackStep(Workflow workflow, WorkflowContent content, object userId,
            StepConfig nextStep, BaseProcessor processor, AutoProcessor autoProcessor)
        {
            FinishData result = new FinishData(workflow);

            #region back

            DateTime now = DateTime.Now;
            TableResolver stepInstResolver = workflow.StepResolver;
            IDbDataSource source = workflow.Source;
            DataRow workflowRow = workflow.WorkflowRow;

            //回退操作
            DataRow stepRow = stepInstResolver.NewRow();
            stepRow.BeginEdit();
            try
            {
                CopyWorkflowToStep(source.Context, workflowRow, stepRow, now, FlowAction.Back);
                CopyManualInfo(workflowRow, stepRow);
            }
            finally
            {
                stepRow.EndEdit();
            }

            //回退步骤 修改接收人列表 和 接收人个数
            //从步骤实例表中查询上次接收的人
            int stepId = GetBackStepId(workflow, nextStep);

            DataRow lastStepRow = stepInstResolver.SelectRowWithKeys(stepId);

            int newIndex = lastStepRow["Index"].Value<int>();
            workflowRow.BeginEdit();
            try
            {
                workflowRow["Index"] = newIndex;

                workflowRow["ReceiveId"] = lastStepRow["ReceiveId"];//上次接收的人
                workflowRow["ReceiveList"] = QuoteStringList.GetQuoteId(lastStepRow["ReceiveId"].ToString());
                workflowRow["ReceiveCount"] = 1;
                workflowRow["SendId"] = userId;
                workflowRow["SendDate"] = now;

                workflowRow["LastManual"] = lastStepRow["LastManual"];
                workflowRow["LastManualName"] = lastStepRow["LastManualName"];
                workflowRow["LastStep"] = lastStepRow["LastStep"];
                workflowRow["LastStepName"] = lastStepRow["LastStepName"];

                //被回退的那个步骤肯定是要作废的
                //lastStepRow["SI_VALID_FLAG"] = 1;

                WorkflowInstResolver.ClearError(workflowRow);

                WorkflowInstResolver.SetWorkflowByStep(now, workflowRow, nextStep);
            }
            finally
            {
                workflowRow.EndEdit();
            }
            result.Resolvers.ApplyData = (transaction) =>
            {
                if (processor != null)
                    processor.ApplyDatas(transaction);
                if (autoProcessor != null)
                    autoProcessor.ApplyDatas(transaction);
                UpdateStepIndex(workflow, newIndex);
            };

            content.Fill(FillContentMode.MainOnly, source);
            TableResolver mainResolver = content.MainTableResolver;
            content.SetMainRowStatus(nextStep, workflow.Config.MainTableColumnPrefix);
            result.Resolvers.AddResolver(mainResolver);

            #endregion back

            workflow.WorkflowResolver.SetCommands(AdapterCommand.Update);
            stepInstResolver.SetCommands(AdapterCommand.Insert);
            mainResolver.SetCommands(AdapterCommand.Update);

            return result;
        }

        private static int GetBackStepId(Workflow workflow, StepConfig nextStep)
        {
            TkDbContext context = workflow.Context;
            DbParameterList dbList = new DbParameterList();
            dbList.Add("SI_WI_ID", TkDataType.Int, workflow.WorkflowId);
            dbList.Add("SI_VALID_FLAG", TkDataType.Int, 0);
            dbList.Add("SI_CURRENT_STEP", TkDataType.String, nextStep.Name);

            string sql = string.Format(ObjectUtil.SysCulture, WHERE_INDEX_SQL, "SI_WI_ID", context.GetSqlParamName("SI_WI_ID"),
               "SI_VALID_FLAG", context.GetSqlParamName("SI_VALID_FLAG"), "SI_CURRENT_STEP",
               context.GetSqlParamName("SI_CURRENT_STEP"));

            sql = string.Format(ObjectUtil.SysCulture,
                "SELECT SI_ID FROM WF_STEP_INST {0} ORDER BY SI_INDEX DESC , SI_ID DESC", sql);
            //string ff = context.ContextConfig.GetListSql("SI_ID", "WF_STEP_INST", new[] { new Toolkit.MetaData.FieldItem("SI_ID") }, sql,
            //    " ORDER BY SI_INDEX DESC , SI_ID DESC ", 0, 1).ListSql;
            // return DataSetUtil.ExecuteScalar(sql, context, dbList).Value<int>();
            return DbUtil.ExecuteScalar(sql, context, dbList).Value<int>();
        }

        private static void UpdateStepIndex(Workflow workflow, int newIndex)
        {
            TkDbContext context = workflow.Context;
            string sql = string.Format(ObjectUtil.SysCulture, UPDATE_INDEX_SQL, context.GetSqlParamName("SI_VALID_FLAG"),
                context.GetSqlParamName("SI_WI_ID"), context.GetSqlParamName("SI_INDEX"),
                context.GetSqlParamName("NEWSI_VALID_FLAG"));
            DbParameterList dbList = new DbParameterList();
            dbList.Add("SI_VALID_FLAG", TkDataType.Int, 1);
            dbList.Add("SI_WI_ID", TkDataType.Int, workflow.WorkflowId);
            dbList.Add("SI_INDEX", TkDataType.Int, newIndex);
            dbList.Add("NEWSI_VALID_FLAG", TkDataType.Int, 0);

            DbUtil.ExecuteNonQuery(sql, workflow.Context, dbList);
        }

        private static FinishData AbortWorkflow(Workflow workflow, FinishType finishType, object userId)
        {
            WorkflowContent content = WorkflowInstResolver.CreateContent(workflow.WorkflowRow);
            content.Fill(FillContentMode.MainOnly, workflow.Source);
            FinishData result = FinishContentStep(workflow, finishType, userId, content, true);
            result.Content = content;
            return result;
        }

        private static FinishData FinishContentStep(Workflow workflow,
            FinishType finishType, object userId, WorkflowContent content, bool isFill)
        {
            return FinishRowStep(workflow, workflow.WorkflowRow, finishType, userId, content, isFill);
        }

        private static void SetFinishData(Workflow workflow, DataRow workflowRow, FinishType finishType,
            object userId, WorkflowContent content, FinishData result)
        {
            DateTime now = DateTime.Now;
            DataRow row = NewWFInstHisEnd(workflowRow, finishType, userId, now, result.WfHisResolver);
            result.WfHistoryRow = row;

            CopyStepToHistory(workflow, workflowRow, now, result.StepHisResolver);
            workflowRow.Delete();

            if (content != null)
            {
                TableResolver mainResolver = SetMainTableEnd(workflow.Config, content, finishType);
                result.Resolvers.AddResolver(mainResolver);
            }
        }

        private static FinishData FinishRowStep(Workflow workflow, DataRow workflowRow,
            FinishType finishType, object userId, WorkflowContent content, bool isFill)
        {
            FinishData result = new FinishData(workflow);
            if (isFill)
            {
                content.FillWithMainData(FillContentMode.MainOnly, workflow.Source);
            }
            SetFinishData(workflow, workflowRow, finishType, userId, content, result);
            workflow.StepResolver.Delete();
            workflow.WorkflowResolver.SetCommands(AdapterCommand.Delete);

            return result;
        }

        public static void AbortAllChild(Workflow workflow, object userId)
        {
            FillChildWorkflow(workflow.WorkflowResolver, workflow.WorkflowId);
            DataRowCollection rows = workflow.WorkflowResolver.HostTable.Rows;

            FinishData result = new FinishData(workflow);

            using (result)
            {
                foreach (DataRow workflowRow in rows)
                {
                    WorkflowContent content = WorkflowInstResolver.CreateContent(workflowRow);
                    content.Fill(FillContentMode.MainOnly, workflow.Source);
                    SetFinishData(workflow, workflowRow, FinishType.Abort, userId, content, result);
                }

                workflow.StepResolver.Delete();
                workflow.WorkflowResolver.SetCommands(AdapterCommand.Delete);

                result.Resolvers.Commit();
            }
        }

        public static FinishData FinishStep(Workflow workflow, WorkflowContent content, EndStepConfig config,
            IEnumerable<TableResolver> tableResolvers, BaseProcessor processor, bool isFill)
        {
            FinishType finishType = (FinishType)((int)config.FinishType);

            FinishData result = FinishContentStep(workflow, finishType,
                workflow.WorkflowRow["LastProcessId"], content, isFill);
            result.Content = content;
            if (processor != null)
            {
                result.Resolvers.ApplyData += processor.ApplyDatas;

                processor.SaveContent(result.WfHistoryRow);
            }
            if (tableResolvers != null)
            {
                result.Resolvers.AddResolvers(tableResolvers);
            }

            if (config.History != null && config.History.IsSaveHistory)
            {
                List<TableResolver> hises = EndHistory(config.History, workflow.Context, workflow.Source, content);
                if (hises.Count > 0)
                {
                    result.Resolvers.AddResolvers(hises);
                    //更新Content
                    result.WfHistoryRow["ContentXml"] = content.CreateXml();
                }
            }

            result.Resolvers.Commit();
            return result;
        }

        private static void CopyStepToHistory(Workflow workflow, DataRow workflowRow, DateTime now,
            TableResolver stepHisRes)
        {
            int wiId = workflowRow["Id"].Value<int>();
            TableResolver stepResolver = workflow.StepResolver;
            int begin;
            if (stepResolver.HostTable != null)
                begin = stepResolver.HostTable.Rows.Count;
            else
                begin = 0;
            stepResolver.SelectWithParam("WiId", wiId);
            int end = stepResolver.HostTable.Rows.Count;
            if (end > begin)
            {
                for (int i = begin; i < end; ++i)
                {
                    DataRow stepNewRow = stepHisRes.NewRow();
                    DataSetUtil.CopyRow(stepResolver.HostTable.Rows[i], stepNewRow);
                }
            }

            //新增一条步骤历史
            DataRow stepRow = stepHisRes.NewRow();
            stepRow.BeginEdit();
            try
            {
                CopyWorkflowToStep(workflow.Context, workflowRow, stepRow, now, FlowAction.Flow);
                //步骤状态需要改变表示已经处理完了
            }
            finally
            {
                stepRow.EndEdit();
            }
            stepHisRes.SetCommands(AdapterCommand.Insert);
        }

        public static void FillChildWorkflow(WorkflowInstResolver workflowResolver, object parentId)
        {
            DataTable table = workflowResolver.HostTable;
            int begin;
            if (table != null)
                begin = table.Rows.Count;
            else
                begin = 0;
            workflowResolver.SelectWithParam("ParentId", parentId);
            int end = table.Rows.Count;
            for (int i = begin; i < end; i++)
            {
                FillChildWorkflow(workflowResolver, table.Rows[i]["Id"]);
            }
        }

        private static DataRow NewWFInstHisEnd(DataRow workflowRow, FinishType finishType,
            object userId, DateTime now, TableResolver wfiHisRes)
        {
            DataRow wfiHisRow = wfiHisRes.NewRow();
            //字段集合（缺少 结束人 和 结束时间）
            DataSetUtil.CopyRow(workflowRow, wfiHisRow, COLUMN_NAMES, COLUMN_NAMES);
            wfiHisRow.BeginEdit();
            try
            {
                //结束时间
                wfiHisRow["EndDate"] = now;
                wfiHisRow["EndState"] = (int)finishType;
                wfiHisRow["EndUser"] = userId;
            }
            finally
            {
                wfiHisRow.EndEdit();
            }
            wfiHisRes.SetCommands(AdapterCommand.Insert);
            return wfiHisRow;
        }

        private static TableResolver SetMainTableEnd(WorkflowConfig workflowConfig, WorkflowContent content,
            FinishType finishType)
        {
            //更新主表
            content.EndMainRowStatus(workflowConfig.MainTableColumnPrefix, finishType);
            TableResolver mainResolver = content.MainTableResolver;
            mainResolver.SetCommands(AdapterCommand.Update);
            return mainResolver;
        }

        private static List<TableResolver> EndHistory(HistoryConfig historyConfig,
            TkDbContext context, IDbDataSource source, WorkflowContent content)
        {
            List<TableResolver> resolvers = new List<TableResolver>();
            if (historyConfig.IsSaveHistory)
            {
                foreach (HistoryTableConfig history in historyConfig.HistoryTables)
                {
                    TableResolver fromResolver = null;
                    ContentItem item = content.ContentItems[history.TableName];
                    if (item != null)
                    {
                        if (!item.IsMain)
                        {
                            item.FillData(source);
                        }
                        fromResolver = item.GetTableResolver();
                    }

                    if (fromResolver != null)
                    {
                        TableResolver toResolver = history.ResolverCreator.CreateObject(source);
                        toResolver.SelectTableStructure();
                        DataSetUtil.CopyDataTable(fromResolver.HostTable, toResolver.HostTable);
                        fromResolver.Delete(true);
                        toResolver.SetCommands(AdapterCommand.Insert);
                        content.AddHistoryInfo(history.TableName, history.ResolverCreator);
                        resolvers.Add(fromResolver);
                        resolvers.Add(toResolver);
                        if (item.IsMain)
                        {
                            if (toResolver.HostTable.Rows.Count > 0)
                                content.SetMainRow(toResolver.HostTable.Rows[0]);
                        }
                    }
                }
            }
            return resolvers;
        }

        #endregion EndStep

        public static void NoifyParentStep(TkDbContext context, string parentId)
        {
            Workflow parentWorkflow = Workflow.CreateWorkflow(context, parentId);
            using (parentWorkflow)
            {
                parentWorkflow.Run();
            }
        }

        public static void Add(this QuoteStringList list, IEnumerable<IUser> users)
        {
            foreach (IUser user in users)
                list.Add(user.Id);
        }

        public static T ExecuteExpression<T>(string expression, DataRow mainRow, DataRow workflowRow)
        {
            return EvaluatorUtil.Execute<T>(expression, ("dataRow", mainRow), ("workflowRow", workflowRow));
        }
    }
}