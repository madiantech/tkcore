using System;
using System.Data;
using System.Text;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal static class WorkflowExtension
    {
        private const string MY_WORK_URL = "~/c/plugin/C/WfMyWork";

        private static string EncodeConfig(string config)
        {
            byte[] data = Encoding.UTF8.GetBytes(config);
            return HttpUtility.UrlEncode(Convert.ToBase64String(data), Encoding.UTF8);
        }

        public static string GetContentUrl(this Workflow workflow)
        {
            return GetContentUrl(workflow, false);
        }

        public static string GetContentUrl(this Workflow workflow, bool isFull)
        {
            StepConfig step = workflow.CurrentStep.Config;
            IContentXmlConfig contentXml = step as IContentXmlConfig;
            ProcessDisplay display = workflow.Config.ProcessDisplay;
            if (contentXml != null)
            {
                string config = contentXml.ContentXmlConfig;
                string full = isFull ? "&Full=content" : string.Empty;
                return string.Format(ObjectUtil.SysCulture,
                    "~/c/wfc/C/content?WfId={1}&Source={0}{3}{2}", EncodeConfig(config),
                    workflow.WorkflowId, full, GetProcessDisplay(display));
            }
            throw new ErrorPageException("错误调用", "当前的步骤不具备ContentXml的配置，不能调用该功能");
        }

        private static string GetProcessDisplay(ProcessDisplay display)
        {
            return "&Display=" + display;
        }

        public static string GetHistoryContentUrl(TkDbContext context, string wfId)
        {
            const string sql = "SELECT WI_WD_NAME FROM WF_WORKFLOW_INST_HIS";
            IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, "WI_ID", TkDataType.Int, wfId);
            string name = DbUtil.ExecuteScalar(sql, context, builder).ToString();
            WorkflowConfig config = CacheManager.GetItem("WorkflowConfig", name,
                context).Convert<WorkflowConfig>();
            ProcessDisplay display = config.ProcessDisplay;

            if (string.IsNullOrEmpty(config.ContentXmlConfig))
                throw new ErrorPageException("流程配置有问题", string.Format(ObjectUtil.SysCulture,
                    "流程{0}没有配置全局的ContextXMLConfig，无法展示", name));

            return string.Format(ObjectUtil.SysCulture,
                "~/c/wfc/C/history?WfId={1}&Source={0}{2}", EncodeConfig(config.ContentXmlConfig),
                wfId, GetProcessDisplay(display));
        }

        public static string GetWorkflowUrl(this Workflow workflow)
        {
            string stepName;
            ManualStep currManualStep = workflow.CurrentStep as ManualStep;
            if (currManualStep != null)
                stepName = currManualStep.Config.Name;
            else
                stepName = string.Empty;
            if (workflow.WorkflowRow.RowState == DataRowState.Deleted || workflow.WorkflowRow.RowState == DataRowState.Detached)
                return MY_WORK_URL;
            string wdName = workflow.WorkflowRow["WdName"].ToString();
            TkDbContext context = workflow.Context;
            if (workflow.IsUserStep(BaseGlobalVariable.UserId.ToString()))
            {
                string config = (currManualStep.Config as ManualStepConfig).ProcessXmlConfig; // ContentXmlConfig
                return string.Format(ObjectUtil.SysCulture,
                    "~/c/wfp/C/a?WfId={1}&Source={0}", EncodeConfig(config), workflow.WorkflowId);
            }
            else
            {
                return GetNextWFUrl(context, wdName, stepName);
            }
        }

        public static string GetNextWFUrl(TkDbContext context, string wdName, string stepName)
        {
            if (!string.IsNullOrEmpty(stepName))
            {
                IParamBuilder builderIns = WorkflowUtil.CreateInsByStep(context,
                    BaseGlobalVariable.UserId, wdName, stepName);
                string sql = context.ContextConfig.GetListSql("WI_ID", "WF_WORKFLOW_INST",
                    new Toolkit.MetaData.FieldItem[] { (Toolkit.MetaData.FieldItem)"WI_ID" }, "WHERE" + builderIns.Sql,
                    string.Empty, 0, 1).ListSql;

                DataSet ds = new DataSet() { Locale = ObjectUtil.SysCulture };
                SqlSelector selector = new SqlSelector(context, ds);
                using (selector)
                using (ds)
                {
                    selector.Select("WF_WORKFLOW_INST", sql, builderIns.Parameters);
                    if (ds.Tables["WF_WORKFLOW_INST"].Rows.Count > 0)
                    {
                        int insId = ds.Tables["WF_WORKFLOW_INST"].Rows[0]["WI_ID"].Value<int>();
                        if (insId > 0)
                        {
                            return string.Format(ObjectUtil.SysCulture, "~/c/~source/C/WfStatus?WfId={0}", insId);
                        }
                    }
                }
            }
            return MY_WORK_URL;
        }
    }
}