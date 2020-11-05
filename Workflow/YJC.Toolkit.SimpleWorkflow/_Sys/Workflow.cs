using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class Workflow : IDisposable
    {
        private const string WORKFLOW_TABLENAME = "WF_WORKFLOW_INST";
        private readonly IDbDataSource fSource;
        private readonly TkDbContext fContext;
        private readonly WorkflowInstResolver fWorkflowResolver;
        private StepInstResolver fStepResolver;

        private readonly bool fIsNewSource;

        private Workflow(IDbDataSource workflowSource)
        {
            fContext = workflowSource.Context;
            fSource = workflowSource;
            fWorkflowResolver = new WorkflowInstResolver(fSource);
        }

        private Workflow(TkDbContext context)
            : this(new WorkflowSource(context, false))
        {
            fIsNewSource = true;
        }

        private Workflow(TkDbContext context, string name, IParameter parameter, string createUser,
            int? parentId, string parentHint)
            : this(context)
        {
            Config = CacheManager.GetItem("WorkflowConfig", name, context).Convert<WorkflowConfig>();
            //WorkflowConfigCacheCreator.Creator, context);
            WorkflowRow = fWorkflowResolver.NewRow();
            WorkflowRow.BeginEdit();
            try
            {
                string id = context.GetUniId(fWorkflowResolver.TableName);
                WorkflowRow["Id"] = id;
                WorkflowId = int.Parse(id, ObjectUtil.SysCulture);
                WorkflowRow["WdName"] = name;
                WorkflowRow["CreateUser"] = createUser;
                WorkflowRow["Retrievable"] = Config.Retrievable;
                //初始化参与人列表
                WorkflowRow["RefList"] = QuoteStringList.GetQuoteId(createUser);
                WorkflowRow["CreateDate"] = DateTime.Now;
                if (parentId != null)
                {
                    WorkflowRow["PcFlag"] = (int)WorkflowType.Child;
                    WorkflowRow["ParentId"] = parentId;
                    WorkflowRow["LastDisplayName"] = string.IsNullOrEmpty(parentHint) ? "开始" : parentHint;
                }
            }
            finally
            {
                WorkflowRow.EndEdit();
            }
            BeginStep step = Config.Steps.BeginStep.CreateStep(this) as BeginStep;
            step.Parameter = parameter;
            CurrentStep = step;
            CurrentState = BeginNRState.Instance;
        }

        private Workflow(TkDbContext context, string name, IParameter parameter, string createUser)
            : this(context, name, parameter, createUser, null, null)
        {
        }

        private Workflow(string workflowId, IDbDataSource workflowSource)
            : this(workflowSource)
        {
            DataRow row = fWorkflowResolver.SelectRowWithKeys(workflowId);
            WorkflowId = int.Parse(workflowId, ObjectUtil.SysCulture);
            WorkflowRow = row;
            Config = CacheManager.GetItem("WorkflowConfig", row["WdName"].Value<string>(),
                workflowSource.Context).Convert<WorkflowConfig>();
            //WorkflowConfigCacheCreator.Creator, workflowSource.Context);
            StepConfig stepConfig = Config.Steps[row["CurrentStep"].Value<string>()];
            CurrentStep = stepConfig.CreateStep(this);
            CurrentState = CurrentStep.GetState(row["Status"].Value<StepState>());
        }

        private Workflow(TkDbContext context, string workflowId)
            : this(workflowId, new WorkflowSource(context))
        {
        }

        #region IDisposable 成员

        public void Dispose()
        {
            // fSource.Dispose();
            if (fIsNewSource)
            {
                fSource.DisposeObject();
            }
            fWorkflowResolver.Dispose();
            if (fStepResolver != null)
                fStepResolver.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        internal WorkflowInstResolver WorkflowResolver
        {
            get
            {
                return fWorkflowResolver;
            }
        }

        internal StepInstResolver StepResolver
        {
            get
            {
                if (fStepResolver == null)
                    fStepResolver = new StepInstResolver(fSource);
                return fStepResolver;
            }
        }

        public int WorkflowId { get; private set; }

        public WorkflowConfig Config { get; private set; }

        public Step CurrentStep { get; private set; }

        internal State CurrentState { get; private set; }

        public DataRow WorkflowRow { get; private set; }

        public TkDbContext Context
        {
            get
            {
                return fContext;
            }
        }

        public IDbDataSource Source
        {
            get
            {
                return fSource;
            }
        }

        public bool IsFinish { get; internal set; }

        public bool IsManualStep
        {
            get
            {
                if (CurrentStep != null)
                    return CurrentStep.Config.StepType == StepType.Manual;
                return false;
            }
        }

        internal void UpdateState(State state)
        {
            TkDebug.AssertArgumentNull(state, "state", this);

            CurrentState = state;
        }

        public void UpdateStep(Step step)
        {
            TkDebug.AssertArgumentNull(step, "step", this);

            //if (CurrentStep != null)
            //    CurrentStep.Dispose();
            CurrentStep = step;
        }

        public void Run()
        {
            bool continueRun;
            do
            {
                try
                {
                    if (!IsFinish)
                    {
                        continueRun = CurrentState.Execute(this);
                    }
                    else
                    {
                        continueRun = false;
                    }
                }
                catch (WorkflowException ex)
                {
                    SaveError(ex);
                    continueRun = SetWorkflowException(ex);
                }
                catch (Exception ex)
                {
                    // throw ex;
                    //throw new ToolkitException("工作流异常", ex, this);
                    SaveError(ex);
                    continueRun = false;
                    IsFinish = true;
                    StepUtil.ErrorAbort(this, FinishType.Error);
                }
            } while (continueRun);
        }

        private void SetWorkflowRowByState()
        {
            if (CurrentState.StepState == StepState.ProcessNotSend)
            {
                WorkflowRow["CurrentStep"] = WorkflowRow["LastStep"];
                WorkflowRow["CurrentStepName"] = WorkflowRow["LastStepName"];
                WorkflowRow["StepType"] = CurrentStep.Config.StepType;
            }
        }

        private void SaveError(Exception ex)
        {
            int errorId;
            WorkflowException wfException = ex as WorkflowException;
            if (wfException == null || wfException != null && wfException.InnerException != null)
                errorId = GetSaveError(ex);
            else
                errorId = 0;
            WorkflowRow.BeginEdit();
            try
            {
                SetWorkflowRowByState();
                if (errorId > 0)
                {
                    WorkflowRow["WeId"] = errorId;
                }
            }
            finally
            {
                WorkflowRow.EndEdit();
            }
        }

        private int GetSaveError(Exception ex)
        {
            //int errorId = DataSetUtil.SaveError(Context, WORKFLOW_TABLENAME, WorkflowRow["WI_ID"].ToString(),
            //    "Workflow", "0", ex);
            //return errorId;
            ExceptionUtil.LogException(new ExceptionData("Workflow", null, null, ex));
            return 0;
        }

        private bool SetWorkflowException(WorkflowException wfException)
        {
            bool continueRun;
            switch (wfException.ErrorConfig.ProcessType)
            {
                case ErrorProcessType.Abort:
                    //WorkflowResolver.ta
                    StepUtil.ErrorAbort(this, FinishType.Error);
                    continueRun = false;
                    IsFinish = true;
                    break;

                case ErrorProcessType.Retry:
                    if (WorkflowRow["RetryTimes"].Value<int>() < wfException.ErrorConfig.RetryTimes)
                    {
                        //保存重试信息
                        WorkflowInstResolver.SaveError(WorkflowRow, wfException);
                        WorkflowResolver.SetCommands(AdapterCommand.Update);
                        UpdateUtil.UpdateTableResolvers(null, WorkflowResolver);
                        UpdateState(CurrentStep.GetState(StepState.Mistake));
                        continueRun = false;
                    }
                    else
                    {
                        CurrentStep.ClearDataSet();
                        StepUtil.ErrorAbort(this, FinishType.OverTryTimes);
                        continueRun = false;
                        IsFinish = true;
                    }

                    break;

                default:
                    continueRun = false;
                    break;
            }
            return continueRun;
        }

        /// <summary>
        /// 终止流程，强行将流程结束，转入历史 与EndStep类似，但是不执行EndStep配置的插件
        /// </summary>
        /// <param name="userId"></param>
        public void Abort(string userId)
        {
            bool isChild = StepUtil.IsWorkflowType(WorkflowRow, WorkflowType.Child);
            string parentId = WorkflowRow["ParentId"].ToString();

            StepUtil.Abort(this, userId);
            NotifyParentWorkflow(isChild, parentId);
        }

        public void Abort(string userId, bool abortChild)
        {
            if (abortChild)
            {
                bool isChild = StepUtil.IsWorkflowType(WorkflowRow, WorkflowType.Child);
                string parentId = WorkflowRow["ParentId"].ToString();

                StepUtil.AbortAllChild(this, userId);
                NotifyParentWorkflow(isChild, parentId);
            }
            else
                Abort(userId);
        }

        private void NotifyParentWorkflow(bool isChild, string parentId)
        {
            IsFinish = true;
            if (isChild)
                StepUtil.NoifyParentStep(Context, parentId);
        }

        public void Abort(string userId, IEnumerable<TableResolver> tableResolvers, BaseProcessor processor)
        {
            bool isChild = StepUtil.IsWorkflowType(WorkflowRow, WorkflowType.Child);
            string parentId = WorkflowRow["ParentId"].ToString();

            StepUtil.Abort(this, userId, tableResolvers, processor);

            NotifyParentWorkflow(isChild, parentId);
        }

        public bool IsUserStep(string userId)
        {
            Run();
            if (IsManualStep && !IsFinish)
            {
                ManualStep step = CurrentStep as ManualStep;
                return step.ProcessManualWorkflow(WorkflowRow, userId);
            }
            return false;
        }

        internal void UpdateProcessDate()
        {
            fWorkflowResolver.SetCommands(AdapterCommand.Update);
            WorkflowRow["ProcessDate"] = DateTime.Now;
            fWorkflowResolver.UpdateDatabase();
        }

        public void JudgeTimeout()
        {
            //ManualStep step = CurrentStep as ManualStep;
            //bool isTimeout;
            //if (step != null)
            //    isTimeout = step.IsTimeout;
            //else
            //    isTimeout = TimeLimitConfig.IsTimeOut(Config.TimeLimiter, Context,
            //        WorkflowRow, "WI_CREATE_DATE");
            //if (isTimeout)
            //{
            //    WorkflowRow["WI_IS_TIMEOUT"] = true;
            //    WorkflowResolver.SetCommands(AdapterCommand.Update);
            //    WorkflowResolver.UpdateDatabase();
            //}
        }

        public static Workflow CreateWorkflow(TkDbContext context, string name,
            IParameter parameter, string createUser)
        {
            return new Workflow(context, name, parameter, createUser);
        }

        public static Workflow CreateWorkflow(TkDbContext context, string name,
           IParameter parameter, string createUser, int? parentId, string parentHint)
        {
            return new Workflow(context, name, parameter, createUser, parentId, parentHint);
        }

        public static Workflow CreateWorkflow(TkDbContext context, string workflowId)
        {
            return new Workflow(context, workflowId);
        }

        public static Workflow CreateWorkflow(string workflowId, IDbDataSource workflowSource)
        {
            return new Workflow(workflowId, workflowSource);
        }
    }
}