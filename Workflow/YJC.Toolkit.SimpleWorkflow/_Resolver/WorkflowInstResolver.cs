using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowInstResolver : TableResolver
    {
        /// <summary>
        /// 建构函数，设置附着的Xml文件。
        /// </summary>
        /// <param name="hostDataSet">附着的DataSet</param>
        public WorkflowInstResolver(IDbDataSource source)
            : base(MetaDataUtil.CreateTableScheme("WorkflowInst.xml"), source)
        {
        }

        public static WorkflowContent CreateContent(DataRow row)
        {
            string contentXml = row["ContentXml"].ToString();
            TkDebug.AssertNotNullOrEmpty(contentXml, string.Format(ObjectUtil.SysCulture,
                "ID为{0}，模式为{1}的工作流没有设置ContentXml", row["Id"], row["WdName"]), null);

            WorkflowContent content = WorkflowContent.ReadXml(contentXml);
            return content;
        }

        public static void ManualSendWorkflow(DataRow row, object userId, BaseProcessor processor)
        {
            row.BeginEdit();
            try
            {
                // 处理人，处理时间，状态改为PNS
                row["ProcessId"] = userId;
                row["ProcessDate"] = DateTime.Now;
                row["Status"] = (int)StepState.ProcessNotSend;
                processor.SaveContent(row);
            }
            finally
            {
                row.EndEdit();
            }
        }

        public static void SetWorkflowByStep(DataRow row, string currName, string currDisplayName,
            DateTime createDateTime, int stepType, int stepStatus)
        {
            row["CurrentStep"] = currName;
            row["CurrentStepName"] = currDisplayName;
            row["CurrentCreateDate"] = createDateTime;
            row["StepType"] = stepType;
            row["Status"] = stepStatus;
        }

        public static void SetWorkflowByStep(DataRow row, string currName, string currDisplayName,
            DateTime createDateTime, int stepType, int stepStatus, DateTime sendTime)
        {
            row["SendDate"] = sendTime;
            SetWorkflowByStep(row, currName, currDisplayName, createDateTime, stepType, stepStatus);
        }

        public static void SetWorkflowByStep(DataRow row, StepConfig stepConfig)
        {
            DateTime now = DateTime.Now;
            SetWorkflowByStep(row, stepConfig.Name, stepConfig.DisplayName, now,
                (int)stepConfig.StepType, (int)StepState.NotReceive, now);
        }

        public static void SetWorkflowByStep(DateTime now, DataRow row, StepConfig stepConfig)
        {
            SetWorkflowByStep(row, stepConfig.Name, stepConfig.DisplayName, now,
                (int)stepConfig.StepType, (int)StepState.NotReceive, now);
        }

        public static void SetInstInfo(DataRow row, DataRow mainRow, TkDbContext context,
            string infoFieldName, string columnName)
        {
            int sharpP = columnName.IndexOf("#", StringComparison.CurrentCulture);
            if (sharpP > -1)
            {
                string fieldName = columnName.Substring(0, sharpP);
                string easySearchName = columnName.Substring(sharpP + 1);
                EasySearch search = PlugInFactoryManager.CreateInstance<EasySearch>("EasySearch", easySearchName);
                string infoValue = search.Decode(mainRow[fieldName].ToString(), context).Name;
                row[infoFieldName] = infoValue;
            }
            else
            {
                row[infoFieldName] = mainRow[columnName];
            }
        }

        public static void SaveError(DataRow row, WorkflowException wfException)
        {
            ErrorConfig error = wfException.ErrorConfig;
            row.BeginEdit();
            try
            {
                if (row["Status"].Value<StepState>() != StepState.Mistake)
                {
                    row["Status"] = StepState.Mistake;
                    row["ErrorType"] = (int)wfException.Reason;
                    row["MaxRetryTimes"] = error.RetryTimes;
                }
                row["RetryTimes"] = row["RetryTimes"].Value<int>() + 1;
                row["NextExeDate"] = DateTime.Now.Add(error.Interval);
            }
            finally
            {
                row.EndEdit();
            }
        }

        public static void ClearError(DataRow workflowRow)
        {
            //清空错误处理信息 WI_ERROR_TYPE WI_MAX_RETRY_TIMES WI_RETRY_TIMES  WI_NEXT_EXE_DATE
            workflowRow["ErrorType"] = DBNull.Value;
            workflowRow["MaxRetryTimes"] = DBNull.Value;
            workflowRow["RetryTimes"] = DBNull.Value;
            workflowRow["NextExeDate"] = DBNull.Value;
        }
    }
}