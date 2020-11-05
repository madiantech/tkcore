using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ManualStep : Step
    {
        public ManualStep(Workflow workflow, StepConfig config)
            : base(workflow, config)
        {
        }

        protected override bool Execute()
        {
            return false;
        }

        private void SaveWorkflowInst()
        {
            WorkflowResolver.SetCommands(AdapterCommand.Update);
            UpdateUtil.UpdateTableResolvers(Source.Context, null, WorkflowResolver);
        }

        internal bool IsTimeout
        {
            get
            {
                //ManualStepConfig manualConfig = Config as ManualStepConfig;
                //bool isTimeout = TimeLimitConfig.IsTimeOut(manualConfig.TimeLimiter,
                //    Source.Context, WorkflowRow, "WI_CURRENT_CREATE_DATE");
                //if (!isTimeout)
                //    isTimeout = TimeLimitConfig.IsTimeOut(Workflow.Config.TimeLimiter,
                //        Source.Context, WorkflowRow, "WI_CREATE_DATE");
                //return isTimeout;
                return false;
            }
        }

        protected override void Send(StepConfig nextStep)
        {
            if (IsTimeout)
                WorkflowRow["IsTimeout"] = true;
            StepUtil.SendStep(Workflow, nextStep);
        }

        internal override State GetState(StepState state)
        {
            switch (state)
            {
                case StepState.NotReceive:
                    return ManualNRState.Instance;

                case StepState.ReceiveNotOpen:
                    return ManualRNOState.Instance;

                case StepState.OpenNotProcess:
                    return ManualONPState.Instance;

                case StepState.ProcessNotSend:
                    return ManualPNSState.Instance;

                case StepState.Mistake:
                    return ManualMState.Instance;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }

        public bool ProcessManualWorkflow(DataRow workflowRow, string userId)
        {
            StepState state = (StepState)workflowRow["Status"].Value<int>();
            switch (state)
            {
                case StepState.NotReceive:
                    // NR: 检查userId是否在接受人列表中，是接收并打开，返回true，否则返回false
                    string receiveIds = workflowRow["ReceiveList"].ToString();
                    QuoteStringList ulReceive = (QuoteStringList)receiveIds;
                    if (ulReceive.Contains(userId))
                    {
                        workflowRow.BeginEdit();
                        try
                        {
                            workflowRow["ReceiveId"] = userId;
                            workflowRow["Status"] = (int)StepState.OpenNotProcess;
                            workflowRow["ReceiveDate"] = DateTime.Now;
                            //workflowRow["WI_RECEIVE_LIST"] = DBNull.Value;
                            //更新参与人列表
                            string refIds = workflowRow["RefList"].ToString();
                            QuoteStringList ulRef = (QuoteStringList)refIds;
                            ulRef.Add(userId);
                            workflowRow["RefList"] = ulRef.ToString();
                        }
                        finally
                        {
                            workflowRow.EndEdit();
                        }
                        SaveWorkflowInst();
                        return true;
                    }
                    else
                        return false;

                case StepState.ReceiveNotOpen:
                    // RNO: 检查接收人是否是userId，是打开，返回true，否则返回false
                    if (userId == workflowRow["ReceiveId"].ToString())
                    {
                        workflowRow["Status"] = (int)StepState.OpenNotProcess;
                        SaveWorkflowInst();
                        return true;
                    }
                    else
                        return false;

                case StepState.OpenNotProcess:
                    // ONP： 检查接收人是否是userId，是返回true，否则返回false
                    return userId == workflowRow["ReceiveId"].ToString();

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    break;
            }

            return false;
        }

        public void Back(string userId)
        {
            WorkflowContent content = WorkflowInstResolver.CreateContent(WorkflowRow);
            using (content)
            {
                Back(userId, content, null, null);
            }
        }

        public void Unlock()
        {
            // ManualStepConfig manualConfig = Config as ManualStepConfig; manualConfig.
            DataRow row = Workflow.WorkflowRow;
            StepState stepState = row["Status"].Value<StepState>();
            if (stepState == StepState.OpenNotProcess)
            {
                row.BeginEdit();
                try
                {
                    row["Status"] = StepState.NotReceive;
                    row["ReceiveDate"] = DBNull.Value;
                    row["ReceiveId"] = DBNull.Value;
                }
                finally
                {
                    row.EndEdit();
                }
                WorkflowResolver.SetCommands(AdapterCommand.Update);
                UpdateUtil.UpdateTableResolvers(null, WorkflowResolver);
            }
        }

        public bool Back(string userId, WorkflowContent content,
            IEnumerable<TableResolver> interResolvers, BaseProcessor processor)
        {
            string backStepName;
            ManualStepConfig manualConfig = Config as ManualStepConfig;
            if (manualConfig.Back == null || (manualConfig.Back != null
                && string.IsNullOrEmpty(manualConfig.Back.BackStepName)))
            {
                // 否则退到上一人工步骤，如果没有则报错
                string lastManual = WorkflowRow["LastManual"].ToString();
                if (!string.IsNullOrEmpty(lastManual))
                {
                    backStepName = WorkflowRow["LastManual"].ToString();
                }
                else
                {
                    backStepName = WorkflowRow["LastStep"].ToString();
                }

                TkDebug.AssertNotNullOrEmpty(backStepName, string.Format(ObjectUtil.SysCulture,
                    "人工步骤{0}没有可以回退的上一个人工步骤", manualConfig.Name), this);
            }
            else
                backStepName = manualConfig.Back.BackStepName;

            IEnumerable<TableResolver> tableResolvers = null;
            AutoProcessor autoProcessor = null;
            if (manualConfig.Back != null && !string.IsNullOrEmpty(manualConfig.Back.PlugRegName))
            {
                autoProcessor = PlugInFactoryManager.CreateInstance<AutoProcessor>(
                    AutoProcessorPlugInFactory.REG_NAME, manualConfig.Back.PlugRegName);
                autoProcessor.Config = manualConfig;
                autoProcessor.Source = Source;
                autoProcessor.Content = content;
                //填充Content
                content.FillWithMainData(autoProcessor.FillMode, Source);
                try
                {
                    //执行自动操作
                    tableResolvers = autoProcessor.Execute(WorkflowRow);
                }
                catch (Exception ex)
                {
                    throw new PlugInException(Config, manualConfig.Error, ex);
                }
            }

            IEnumerable<TableResolver> updateResolvers = EnumUtil.Convert(interResolvers, tableResolvers);

            StepConfig nextStep = Config.Parent.Steps[backStepName];
            TkDebug.AssertArgumentNull(nextStep, string.Format(ObjectUtil.SysCulture,
                   "指定的人工步骤{0} 不存在", backStepName), this);
            return StepUtil.BackFinish(Workflow, content, userId, updateResolvers, nextStep, processor, autoProcessor);
        }
    }
}