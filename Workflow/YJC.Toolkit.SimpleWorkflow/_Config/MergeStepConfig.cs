using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class MergeStepConfig : StepConfig, IContentXmlConfig
    {
        private ErrorConfig fError;

        protected internal MergeStepConfig()
        {
        }

        internal MergeStepConfig(WorkflowConfig workflowConfig)
            : base(workflowConfig)
        {
        }

        public override StepType StepType
        {
            get
            {
                return StepType.Merge;
            }
        }

        [SimpleElement(UseCData = true)]
        public string MergerConfig { get; internal set; }

        [SimpleElement(UseCData = true)]
        public string ContentXmlConfig { get; internal set; }

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

        internal override Step CreateStep(Workflow workflow)
        {
            return new MergeStep(workflow, this);
        }
    }
}