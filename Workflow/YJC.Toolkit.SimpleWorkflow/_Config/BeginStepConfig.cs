using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class BeginStepConfig : StepConfig
    {
        protected internal BeginStepConfig()
        {
        }

        internal BeginStepConfig(WorkflowConfig workflowConfig)
            : base(workflowConfig)
        {
        }

        public sealed override StepType StepType
        {
            get
            {
                return StepType.Begin;
            }
        }

        public sealed override string Name
        {
            get
            {
                return "_Begin";
            }
            set
            {
            }
        }

        public sealed override string DisplayName
        {
            get
            {
                return "开始";
            }
            internal set
            {
            }
        }

        public sealed override bool HasInStep
        {
            get
            {
                return false;
            }
        }

        [SimpleElement(UseCData = true)]
        public string CreatorConfig { get; internal set; }

        internal override Step CreateStep(Workflow workflow)
        {
            return new BeginStep(workflow, this);
        }
    }
}