using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class EndStepConfig : StepConfig
    {
        private HistoryConfig fHistory;
        private ErrorConfig fError;

        protected internal EndStepConfig()
        {
        }

        internal EndStepConfig(WorkflowConfig workflowConfig)
            : base(workflowConfig)
        {
        }

        public override StepType StepType
        {
            get
            {
                return StepType.End;
            }
        }

        public override string DisplayName
        {
            get
            {
                return "结束";
            }
            internal set
            {
            }
        }

        public override bool HasOutStep
        {
            get
            {
                return false;
            }
        }

        [SimpleElement(UseCData = true)]
        public string ProcessorConfig { get; internal set; }

        [SimpleAttribute]
        public EndType FinishType { get; internal set; } = EndType.Normal;

        [SimpleElement(IsMultiple = true, LocalName = "Notify")]
        public List<string> NotifyList { get; protected set; }

        [ObjectElement]
        public ErrorConfig Error
        {
            get
            {
                if (fError == null)
                    fError = new ErrorConfig();
                return fError;
            }
            internal set
            {
                fError = value;
            }
        }

        [ObjectElement]
        public HistoryConfig History
        {
            get
            {
                if (fHistory == null)
                    fHistory = new HistoryConfig();
                return fHistory;
            }
            internal set
            {
                fHistory = value;
            }
        }

        public void AddNotify(string config)
        {
            TkDebug.AssertArgumentNullOrEmpty(config, "config", this);

            if (NotifyList == null)
                NotifyList = new List<string>();
            NotifyList.Add(config);
        }

        internal void Notify(WorkflowContent content, DataRow workflowRow, IDbDataSource source)
        {
            if (NotifyList != null)
            {
                foreach (string item in NotifyList)
                {
                    var actionConfig = item.ReadJsonFromFactory<IConfigCreator<INotifyAction>>(
                        NotifyActionConfigFactory.REG_NAME);
                    if (actionConfig != null)
                    {
                        INotifyAction action = actionConfig.CreateObject();
                        var userId = workflowRow["CreateUser"].ToString();
                        var userProvider = WorkflowSetting.Current.UserManager.UserProvider;
                        IUser user = userProvider.GetUser(userId);
                        action.DoAction(user, source.Context, workflowRow, content.MainRow);
                    }
                }
            }
        }

        internal override Step CreateStep(Workflow workflow)
        {
            return new EndStep(workflow, this);
        }
    }
}