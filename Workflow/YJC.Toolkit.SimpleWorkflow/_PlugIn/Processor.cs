using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public abstract class Processor : BaseProcessor
    {
        private ManualStepConfig fConfig;
        private StepAction fAction;

        protected Processor()
        {
            fAction = StepAction.Send;
            FillMode = FillContentMode.All;
        }

        public FillContentMode FillMode { get; set; }

        public StepAction Action
        {
            get
            {
                return fAction;
            }
            set
            {
                TkDebug.AssertArgument(value == StepAction.None || value == StepAction.Send,
                    string.Format(ObjectUtil.SysCulture,
                    "Processor的Action只接受None和Send两种情况，当前为{0}，是非法的。", value),
                    "value", this);

                fAction = value;
            }
        }

        public ManualStepConfig ManualConfig
        {
            get
            {
                if (fConfig == null)
                {
                    fConfig = Config as ManualStepConfig;
                    TkDebug.AssertNotNull(fConfig, string.Format(ObjectUtil.SysCulture,
                        "当前步骤的Config必须是人工步骤的配置，现在的名称是{0}，类型是{1}",
                        Config.Name, Config.StepType), this);
                }
                return fConfig;
            }
        }

        public abstract IEnumerable<ResolverConfig> CreateUpdateResolverConfigs(DataRow workflowRow);

        public virtual void FillData(IInputData input, DataRow workflowRow, TableResolver workflowResolver)
        {
        }

        public virtual void OnCommittingData(IInputData input, DataRow workflowRow,
            TableResolver workflowResolver, bool isSave)
        {
        }

        public virtual void OnSending(IInputData input, DataRow workflowRow, TableResolver workflowResolver)
        {
        }
    }
}