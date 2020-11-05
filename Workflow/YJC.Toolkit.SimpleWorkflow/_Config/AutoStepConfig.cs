using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class AutoStepConfig : StepConfig
    {
        private ErrorConfig fError;

        protected internal AutoStepConfig()
        {
        }

        protected internal AutoStepConfig(WorkflowConfig workflowConfig)
            : base(workflowConfig)
        {
        }

        public override StepType StepType
        {
            get
            {
                return StepType.Auto;
            }
        }

        [SimpleElement(UseCData = true)]
        public string ProcessorConfig { get; internal set; }

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
            return new AutoStep(workflow, this);
        }
    }
}