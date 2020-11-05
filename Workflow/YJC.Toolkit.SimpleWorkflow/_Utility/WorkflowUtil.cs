using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public static class WorkflowUtil
    {
        private const string SELECT_SQL = "SELECT COUNT(*) AS COUNT, WI_WD_NAME, WI_CURRENT_STEP, WI_CURRENT_STEP_NAME FROM WF_WORKFLOW_INST";
        private const string TABLE_NAME = "WORKFLOW";
        private const string GROUP_BY = " GROUP BY WI_WD_NAME, WI_CURRENT_STEP, WI_CURRENT_STEP_NAME";

        internal static Workflow CreateWorkflow(TkDbContext context, string name,
            IParameter parameter, string userId, int? parentId, string parentHint)
        {
            Workflow workflow = Workflow.CreateWorkflow(context, name, parameter, userId, parentId, parentHint);
            workflow.Run();
            return workflow;
        }

        internal static Workflow CreateWorkflow(TkDbContext context, string name,
           IParameter parameter, string userId)
        {
            return CreateWorkflow(context, name, parameter, userId, null, null);
        }

        public static bool CreateWorkflow(TkDbContext context, string workflowName, string userId,
            int? parentId, string key, string value, string parentHint = null)
        {
            using (Workflow workflow = CreateWorkflow(context, workflowName,
                Parameter.Create(key, value), userId, parentId, parentHint))
            {
                return workflow.IsUserStep(userId);
            }
        }

        public static bool CreateWorkflow(TkDbContext context, string workflowName, string userId,
            int? parentId, IParameter parameter, string parentHint = null)
        {
            using (Workflow workflow = CreateWorkflow(context, workflowName,
                parameter, userId, parentId, parentHint))
            {
                return workflow.IsUserStep(userId);
            }
        }

        /// <summary>
        /// 只有本人的自动接收
        /// </summary>
        public static void AutoReceive(IDbDataSource source, object userId)
        {
            using (TableResolver instTableResolver = new TableResolver("WF_WORKFLOW_INST", source))
            {
                TkDbContext context = source.Context;
                IParamBuilder typePar = SqlParamBuilder.CreateEqualSql(context, "WI_STEP_TYPE", TkDataType.Int, (int)StepType.Manual);
                IParamBuilder statusPar = SqlParamBuilder.CreateEqualSql(context, "WI_STATUS", TkDataType.Int, (int)StepState.NotReceive);
                IParamBuilder countPar = SqlParamBuilder.CreateEqualSql(context, "WI_RECEIVE_COUNT", TkDataType.Int, 1);
                IParamBuilder listPar = SqlParamBuilder.CreateSingleSql(context, "WI_RECEIVE_LIST", TkDataType.String,
                    "LIKE", QuoteStringList.GetQuoteId(userId.ToString()));
                IParamBuilder allPar = SqlParamBuilder.CreateParamBuilder(typePar, statusPar, countPar, listPar);
                instTableResolver.Select(allPar);

                foreach (DataRow instRow in source.DataSet.Tables["WF_WORKFLOW_INST"].Rows)
                {
                    string refIds = instRow["RefList"].ToString();
                    QuoteStringList ulRef = (QuoteStringList)refIds;
                    ulRef.Add(userId.ToString());
                    instRow.BeginEdit();
                    try
                    {
                        instRow["Status"] = StepState.OpenNotProcess;
                        instRow["ReceiveId"] = userId;
                        instRow["ReceiveDate"] = DateTime.Now;
                        instRow["RefList"] = ulRef.ToString();
                    }
                    finally
                    {
                        instRow.EndEdit();
                    }
                }
                instTableResolver.SetCommands(AdapterCommand.Update);
                UpdateUtil.UpdateTableResolvers(context, (Action<Transaction>)null, instTableResolver);
            }
        }

        private static IParamBuilder GetStepParamBuilder(IDbDataSource source, Tk5TableResolver stepResolver, string workflowId)
        {
            return ParamBuilder.CreateParamBuilder(
                SqlParamBuilder.CreateEqualSql(source.Context, stepResolver["WiId"], workflowId),
                SqlParamBuilder.CreateInSql(source.Context, stepResolver["StepType"], new string[] { "2", "3", "6" }));
        }

        private static IParamBuilder GetStepParamBuilder2(IDbDataSource source, Tk5TableResolver stepResolver, string workflowId)
        {
            return ParamBuilder.CreateParamBuilder(
                SqlParamBuilder.CreateEqualSql(source.Context, stepResolver["WiId"], workflowId),
                SqlParamBuilder.CreateEqualSql(source.Context, stepResolver["StepType"], 3));
        }

        internal static void FillContent(IDbDataSource source, Tk5TableResolver stepResolver,
            string workflowId, Tk5TableResolver workflowRes, WorkflowContent content,
            ProcessDisplay display, DataRow workflowRow, bool isHistory)
        {
            content.Fill(FillContentMode.All, source, isHistory);
            IParamBuilder builder = null;
            switch (display)
            {
                case ProcessDisplay.Normal:
                    builder = GetStepParamBuilder(source, stepResolver, workflowId);
                    break;

                case ProcessDisplay.Parent:
                    TkDebug.Assert(!string.IsNullOrEmpty(workflowRow["ParentId"].ToString()),
                        string.Format(ObjectUtil.SysCulture, "工作流{0}不是子流程，不该被配置成显示Parent的数据",
                        workflowRow["Name"]), null);

                    string subSql = string.Format(ObjectUtil.SysCulture,
                        "SI_WI_ID IN (SELECT WI_ID FROM {0} WHERE WI_ID = {1}) AND SI_STEP_TYPE IN (2, 3, 6)",
                        workflowRes.TableName, workflowRow["ParentId"]);
                    builder = SqlParamBuilder.CreateParamBuilderWithOr(
                        GetStepParamBuilder2(source, stepResolver, workflowId),
                        ParamBuilder.CreateSql(subSql));
                    break;
            }
            //stepResolver.SelectWithParam(string.Empty, "ORDER BY SI_ID DESC", "WiId", workflowId);
            IPageStyle style = (PageStyleClass)PageStyle.Update;
            if (display == ProcessDisplay.Normal || display == ProcessDisplay.Parent)
            {
                stepResolver.Select(builder, "ORDER BY SI_ID DESC");
                stepResolver.Decode(style);
            }
            else if (display == ProcessDisplay.Child)
            {
                // 历史表
                if (isHistory)
                {
                    string subSql = string.Format(ObjectUtil.SysCulture,
                       "SI_WI_ID IN (SELECT WI_ID FROM WF_WORKFLOW_INST_HIS WHERE WI_PARENT_ID = {0}) AND SI_STEP_TYPE = 3",
                       workflowId);
                    builder = SqlParamBuilder.CreateParamBuilderWithOr(
                        GetStepParamBuilder(source, stepResolver, workflowId),
                        ParamBuilder.CreateSql(subSql));
                    stepResolver.Select(builder, "ORDER BY SI_ID DESC");
                }
                else
                {
                    string sql = string.Format(ObjectUtil.SysCulture,
                        "SELECT {0} FROM WF_STEP_INST WHERE (SI_WI_ID = {1} AND SI_STEP_TYPE IN (2, 3, 6)) " +
                        "UNION SELECT {0} FROM WF_STEP_INST_HIS WHERE (SI_WI_ID IN (SELECT WI_ID FROM " +
                        "WF_WORKFLOW_INST_HIS WHERE WI_PARENT_ID = {1}) AND SI_STEP_TYPE = 3) ORDER BY SI_ID DESC",
                        stepResolver.Fields, workflowId);
                    SqlSelector.Select(source.Context, source.DataSet, stepResolver.TableName, sql);
                }
                stepResolver.Decode(style);
            }
            workflowRes.Decode(style);
        }

        internal static WorkflowContent FillContent(IDbDataSource source,
            Tk5TableResolver stepResolver, string workflowId, Tk5TableResolver workflowRes,
            ProcessDisplay display, bool isHistory)
        {
            DataRow row = workflowRes.SelectRowWithKeys(workflowId);
            WorkflowContent content = WorkflowInstResolver.CreateContent(row);
            FillContent(source, stepResolver, workflowId, workflowRes, content,
                display, row, isHistory);
            return content;
        }

        internal static IParamBuilder CreateInsByStep(TkDbContext context, object userId, string wdName,
            string stepName)
        {
            IParamBuilder builder = SqlParamBuilder.CreateParamBuilder(
                SqlParamBuilder.CreateEqualSql(context, "WI_WD_NAME", TkDataType.String, wdName),
                SqlParamBuilder.CreateEqualSql(context, "WI_CURRENT_STEP", TkDataType.String, stepName));
            IParamBuilder builder1 = WorkflowUtil.CreateUserFilter(context, userId);
            return SqlParamBuilder.CreateParamBuilder(builder, builder1);
        }

        internal static IParamBuilder CreateInsByStep(TkDbContext context, object userId, string wdName,
            string stepName, string name)
        {
            List<IParamBuilder> paras = new List<IParamBuilder>();
            if (!string.IsNullOrEmpty(wdName))
            {
                paras.Add(SqlParamBuilder.CreateEqualSql(context, "WI_WD_NAME", TkDataType.String, wdName));
            }
            if (!string.IsNullOrEmpty(stepName))
            {
                paras.Add(SqlParamBuilder.CreateEqualSql(context, "WI_CURRENT_STEP", TkDataType.String, stepName));
            }
            if (!string.IsNullOrEmpty(name))
            {
                paras.Add(SqlParamBuilder.CreateSingleSql(context, "WI_NAME", TkDataType.String,
                "LIKE", string.Format(ObjectUtil.SysCulture, "%{0}%", name)));
            }
            paras.Add(WorkflowUtil.CreateUserFilter(context, userId));
            return SqlParamBuilder.CreateParamBuilder(paras);
        }

        internal static IParamBuilder CreateUserFilter(TkDbContext context, object userId)
        {
            IParamBuilder receiveBuilder = SqlParamBuilder.CreateEqualSql(context, "WI_RECEIVE_ID",
                TkDataType.Int, userId);

            IParamBuilder stepBuilder = SqlParamBuilder.CreateParamBuilder(
                SqlParamBuilder.CreateEqualSql(context, "WI_STEP_TYPE", TkDataType.Int, (int)StepType.Manual),
                SqlParamBuilder.CreateEqualSql(context, "WI_STATUS", TkDataType.Int, (int)StepState.NotReceive),
                SqlParamBuilder.CreateSingleSql(context, "WI_RECEIVE_COUNT", TkDataType.Int, ">", 1),
                SqlParamBuilder.CreateSingleSql(context, "WI_RECEIVE_LIST", TkDataType.String,
                    "LIKE", string.Format(ObjectUtil.SysCulture, "%{0}%", QuoteStringList.GetQuoteId(userId.ToString()))));
            return SqlParamBuilder.CreateParamBuilderWithOr(receiveBuilder, stepBuilder);
        }

        internal static IParamBuilder CreateUserFilter(TkDbContext context, object userId, IInputData input)
        {
            IParamBuilder receiveBuilder = SqlParamBuilder.CreateEqualSql(context, "WI_RECEIVE_ID",
                TkDataType.Int, userId);

            IParamBuilder stepBuilder = SqlParamBuilder.CreateParamBuilder(
                SqlParamBuilder.CreateEqualSql(context, "WI_STEP_TYPE", TkDataType.Int, (int)StepType.Manual),
                SqlParamBuilder.CreateEqualSql(context, "WI_STATUS", TkDataType.Int, (int)StepState.NotReceive),
                SqlParamBuilder.CreateSingleSql(context, "WI_RECEIVE_COUNT", TkDataType.Int, ">", 1),
                SqlParamBuilder.CreateSingleSql(context, "WI_RECEIVE_LIST", TkDataType.String,
                    "LIKE", string.Format(ObjectUtil.SysCulture, "%{0}%", QuoteStringList.GetQuoteId(userId.ToString()))));

            if (input != null)
            {
                string name = input.QueryString["Name"];
                if (!string.IsNullOrEmpty(name))
                {
                    IParamBuilder sp3 = SqlParamBuilder.CreateSingleSql(context, "WI_NAME", TkDataType.String,
                        "LIKE", $"%{name.Trim()}%");

                    return SqlParamBuilder.CreateParamBuilder(sp3,
                        SqlParamBuilder.CreateParamBuilderWithOr(receiveBuilder, stepBuilder));
                }
            }
            return SqlParamBuilder.CreateParamBuilderWithOr(receiveBuilder, stepBuilder);
        }

        internal static void SelectWorkflowStep(SqlSelector fSelector, object userId, IInputData input = null)
        {
            if (input == null)
            {
                fSelector.Select(TABLE_NAME, SELECT_SQL,
                    WorkflowUtil.CreateUserFilter(fSelector.Context, userId), GROUP_BY);
            }
            else
            {
                fSelector.Select(TABLE_NAME, SELECT_SQL,
                    WorkflowUtil.CreateUserFilter(fSelector.Context, userId, input), GROUP_BY);
            }

            var list = (from row in fSelector.HostDataSet.Tables[TABLE_NAME].AsEnumerable()
                        select row.Field<string>("WI_WD_NAME")).Distinct().ToArray();
            if (list.Length > 0)
            {
                MetaData.FieldItem item = new MetaData.FieldItem("WD_SHORT_NAME", TkDataType.String);
                IParamBuilder builder = SqlParamBuilder.CreateInSql(fSelector.Context, item, list);
                fSelector.Select("WF_WORKFLOW_DEF",
                    "SELECT WD_SHORT_NAME CODE_VALUE, WD_NAME CODE_NAME FROM WF_WORKFLOW_DEF", builder);
            }
        }

        /// <summary>
        /// WI_RETRIEVABLE,WI_STATUS,WI_STEP_TYPE,WI_LAST_STEP,WI_LAST_MANUAL,WI_LAST_PROCESS_ID
        /// </summary>
        /// <param name="workflowRow"></param>
        /// <param name="userId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsEnableFetch(DataRow workflowRow, string userId, TkDbContext context)
        {
            bool isNR = workflowRow["Status"].Value<StepState>() == StepState.NotReceive;
            //当前步骤为NR
            if (isNR && workflowRow["Retrievable"].Value<bool>())
            {
                StepType currentStepType = workflowRow["StepType"].Value<StepType>();
                bool isManualOrMerger = currentStepType == StepType.Manual || currentStepType == StepType.Merge;
                //当前步骤是 人工步骤 或者 聚合步骤
                if (isManualOrMerger)
                {
                    string lastStepName = workflowRow["LastStep"].ToString();
                    // int preLength = wf.CurrentStep.Config.PrevSteps.Count();
                    WorkflowConfig wfConfig = CacheManager.GetItem("WorkflowConfig", workflowRow["WdName"].ToString(),
                        context).Convert<WorkflowConfig>();
                    //WorkflowConfigCacheCreator.Creator, context);
                    StepConfig lastStepConfig = wfConfig.Steps[lastStepName];
                    //bool isLastManual = false;
                    if (lastStepConfig != null)
                    {
                        //上一步骤是否为上一人工步骤
                        if (workflowRow["LastStep"].ToString().Trim() == workflowRow["LastManual"].ToString().Trim())
                        {
                            //上一步骤处理人 为当前用户
                            if (workflowRow["LastProcessId"].ToString() == userId)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //public static TimeSpan LimitSetPro(TimeLimitConfig limitConfig, DataRow workflowRow, TkDbContext context, string wfTimeField)
        //{
        //    if (limitConfig != null)
        //    {
        //        if (limitConfig.Enabled)
        //        {
        //            DateTime now = DateTime.Now;
        //            TimeSpan timeSpan = limitConfig.Time;
        //            DateTime beginTime = workflowRow[wfTimeField].Value<DateTime>();
        //            TimeSpan diffTime = TimeSpan.Zero;
        //            if (!limitConfig.IsWorkDay)
        //            {
        //                diffTime = now - beginTime;
        //            }
        //            else
        //            {
        //                diffTime = HolidayUtil.GetWorkDays(beginTime, now, context);
        //            }
        //            TimeSpan overDiff = diffTime - timeSpan;
        //            return overDiff;
        //        }
        //    }
        //    return TimeSpan.Zero;
        //}

        //public static TimeSpan WorkflowLimitSet(TimeLimitConfig limitConfig, DataRow workflowRow, TkDbContext context)
        //{
        //    return LimitSetPro(limitConfig, workflowRow, context, "WI_CREATE_DATE");
        //}
    }
}