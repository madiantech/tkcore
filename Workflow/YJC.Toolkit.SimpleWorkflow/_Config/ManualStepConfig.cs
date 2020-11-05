using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ManualStepConfig : StepConfig, IContentXmlConfig
    {
        private ProcessConfig fProcess;

        //private ErrorConfig fError;

        protected internal ManualStepConfig()
        {
        }

        internal ManualStepConfig(WorkflowConfig workflowConfig)
            : base(workflowConfig)
        {
        }

        public sealed override StepType StepType
        {
            get
            {
                return StepType.Manual;
            }
        }

        [SimpleAttribute]
        public bool ContainsSave { get; internal set; }

        [SimpleElement(UseCData = true)]
        public string ContentXmlConfig { get; internal set; }

        [SimpleElement(UseCData = true)]
        public string ProcessXmlConfig { get; internal set; }

        [SimpleAttribute]
        public bool UseApprove { get; internal set; }

        [ObjectElement]
        public ErrorConfig Error { get; internal set; }

        [SimpleElement]
        public string ActorConfig { get; set; }

        //[ObjectElement]
        //public TimeLimitConfig TimeLimiter { get; internal set; }

        [ObjectElement]
        public ProcessConfig Process
        {
            get
            {
                if (fProcess == null)
                    fProcess = new ProcessConfig();
                return fProcess;
            }
            internal set
            {
                fProcess = value;
            }
        }

        [ObjectElement]
        public BackConfig Back { get; internal set; }

        [SimpleElement(IsMultiple = true, LocalName = "Notify")]
        public List<string> NotifyList { get; protected set; }

        public string ProcessXmlWithoutExtension
        {
            get
            {
                return Path.ChangeExtension(ProcessXmlConfig, null);
            }
        }

        internal override Step CreateStep(Workflow workflow)
        {
            return new ManualStep(workflow, this);
        }

        public override void Prepare(WorkflowContent content, DataRow workflowRow, IDbDataSource source)
        {
            //先检查是否指定了 接受人
            if (string.IsNullOrEmpty(workflowRow["AdminData"].ToString()))
            {
                SetReceiveUserList(content, workflowRow, source);
            }
            else
            {
                workflowRow["ReceiveList"] = workflowRow["AdminData"];
                workflowRow["AdminData"] = DBNull.Value;
            }
            //提醒
            //string selfId = workflowRow["WI_PROCESS_ID"].ToString();
            //WorkerThread wt = WorkerThread.SysThread;
            //IContentProvider contentProvider = null;
            //foreach (NotifyConfig notifyConfig in Notifies)
            //{
            //    if (string.IsNullOrEmpty(notifyConfig.ContentProvider))
            //    {
            //        //默认的邮件发送 content
            //        contentProvider = new EmailContent();
            //    }
            //    else
            //    {
            //        contentProvider = PlugInFactoryManager.CreateInstance<IContentProvider>(
            //         ContentProviderFactory.FACTORY_NAME, notifyConfig.ContentProvider);
            //    }
            //    WorkflowContent content = WorkflowInstResolver.CreateContent(workflowRow);
            //    using (content)
            //    {
            //        content.Fill(contentProvider.FillMode, source);
            //        string contentString = contentProvider.CreateContent(workflowRow, content.MainRow, source.DataSet);
            //        INotifyAction notifyAction = PlugInFactoryManager.CreateInstance<INotifyAction>(
            //            NotifyActionPlugInFactory.FACTORY_NAME, notifyConfig.NotifyAction);
            //        string userIds = workflowRow["WI_RECEIVE_LIST"].ToString();
            //        foreach (string userId in userIds.Split(','))
            //        {
            //            if (notifyConfig.NotSendSelf && userId == selfId)
            //            {
            //                continue;
            //            }
            //            string uid = userId.Replace("\"", "");
            //            IUser user = UserProvider.GetUser(uid);
            //            Action<IUser, string> action = notifyAction.DoAction;
            //            wt.BeginInvoke(null, action, new object[] { user, contentString }, null);
            //            // notifyAction.DoAction(user, contentString);
            //        }
            //    }
            //}

            if (NotifyList != null)
            {
                foreach (string item in NotifyList)
                {
                    var actionConfig = item.ReadJsonFromFactory<IConfigCreator<INotifyAction>>(
                        NotifyActionConfigFactory.REG_NAME);
                    if (actionConfig != null)
                    {
                        INotifyAction action = actionConfig.CreateObject();
                        QuoteStringList userList = workflowRow["ReceiveList"].Value<QuoteStringList>();
                        var userProvider = WorkflowSetting.Current.UserManager.UserProvider;
                        var users = userList.CreateEnumerable();
                        foreach (string userId in users)
                        {
                            IUser user = userProvider.GetUser(userId);
                            action.DoAction(user, source.Context, workflowRow, content.MainRow);
                        }
                    }
                }
            }
        }

        public void AddNotify(string config)
        {
            TkDebug.AssertArgumentNullOrEmpty(config, "config", this);

            if (NotifyList == null)
                NotifyList = new List<string>();
            NotifyList.Add(config);
        }

        private void SetReceiveUserList(WorkflowContent content, DataRow workflowRow,
            IDbDataSource source)
        {
            QuoteStringList list = new QuoteStringList();
            var stepUserConfig = ActorConfig.ReadJsonFromFactory<IConfigCreator<IManualStepUser>>(
                ManualStepUserConfigFactory.REG_NAME);
            TkDebug.AssertNotNull(stepUserConfig, string.Format(ObjectUtil.SysCulture,
                "步骤{0}没有设置用户", DisplayName), this);
            IManualStepUser stepUser = stepUserConfig.CreateObject();
            var result = stepUser.GetUserList(content, workflowRow, source);
            list.Add(result);

            workflowRow.BeginEdit();
            try
            {
                workflowRow["ReceiveList"] = list.ToString();
                workflowRow["ReceiveCount"] = list.Count;
            }
            finally
            {
                workflowRow.EndEdit();
            }
            if (list.Count == 0)
            {
                throw new NoActorException(this, Error);
            }
        }
    }
}