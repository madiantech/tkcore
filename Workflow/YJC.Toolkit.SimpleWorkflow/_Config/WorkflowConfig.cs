using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [DayChangeCache]
    [RegClass(Author = "YJC", Description = "工作流配置", CreateDate = "2019-12-23")]
    public sealed class WorkflowConfig : ToolkitConfig, IContentXmlConfig
    {
        private static readonly QName ROOT = QName.Get("Workflow");

        private readonly StepConfigCollection fSteps;

        public WorkflowConfig()
        {
            fSteps = new StepConfigCollection();
            ProcessDisplay = ProcessDisplay.Normal;
        }

        protected override void OnReadObject()
        {
            var manualStep = from step in fSteps
                             where step.StepType == StepType.Manual && ((ManualStepConfig)step).UseApprove
                             select step;
            UseApprove = manualStep.Any();
        }

        [SimpleAttribute]
        internal int Id { get; set; }

        [SimpleAttribute]
        public string Name { get; internal set; }

        [SimpleAttribute]
        public bool Retrievable { get; internal set; }

        [SimpleAttribute]
        public string DisplayName { get; internal set; }

        [SimpleAttribute]
        public string Description { get; internal set; }

        [SimpleAttribute(DefaultValue = ProcessDisplay.Normal)]
        public ProcessDisplay ProcessDisplay { get; internal set; }

        [SimpleAttribute]
        public WorkflowPriority Priority { get; internal set; }

        [SimpleAttribute]
        public string MainTableColumnPrefix { get; internal set; }

        [SimpleAttribute]
        public string OtherColumnOne { get; internal set; }

        [SimpleAttribute]
        public string OtherColumnOneDisplayName { get; internal set; }

        [SimpleAttribute]
        public string OtherColumnTwo { get; internal set; }

        [SimpleAttribute]
        public string OtherColumnTwoDisplayName { get; internal set; }

        [SimpleElement(UseCData = true)]
        public string ContentXmlConfig { get; internal set; }

        [SimpleAttribute]
        public bool IsSaveContent { get; internal set; }

        public bool UseApprove { get; private set; }

        //[ObjectElement]
        //public TimeLimitConfig TimeLimiter { get; internal set; }

        [DynamicElement(WorkflowStepConfigFactory.REG_NAME, IsMultiple = true)]
        public StepConfigCollection Steps
        {
            get
            {
                return fSteps;
            }
        }

        internal StepConfig CreateStepConfig(StepType type)
        {
            switch (type)
            {
                case StepType.Begin:
                    return new InternalBeginStepConfig(this);

                case StepType.End:
                    return new InternalEndStepConfig(this);

                case StepType.Manual:
                    return new InternalManualStepConfig(this);

                case StepType.Route:
                    return new InternalRouteStepConfig(this);

                case StepType.Auto:
                    return new InternalAutoStepConfig(this);

                case StepType.Merge:
                    return new InternalMergeStepConfig(this);

                default:
                    return null;
            }
        }

        public string CreateXml()
        {
            return this.WriteXml(WorkflowConst.WriteSettings, ROOT);
        }

        public string CreateJson()
        {
            return this.WriteJson(WorkflowConst.WriteSettings);
        }

        public static WorkflowConfig ReadXml(string xml)
        {
            TkDebug.AssertArgumentNullOrEmpty(xml, "xml", null);

            WorkflowConfig content = new WorkflowConfig();
            content.ReadXml(xml, WorkflowConst.ReadSettings, ROOT);
            return content;
        }

        public static WorkflowConfig ReadJson(string json)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, "json", null);
            WorkflowConfig content = new WorkflowConfig();
            content.ReadJson(json, WorkflowConst.ReadSettings);
            return content;
        }
    }
}