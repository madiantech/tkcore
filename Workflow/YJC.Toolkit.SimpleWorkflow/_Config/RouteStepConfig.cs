using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class RouteStepConfig : StepConfig
    {
        private readonly List<ConnectionConfig> fConnections = new List<ConnectionConfig>();
        private ErrorConfig fError;

        protected internal RouteStepConfig()
        {
        }

        internal RouteStepConfig(WorkflowConfig workflowConfig)
            : base(workflowConfig)
        {
        }

        public sealed override StepType StepType
        {
            get
            {
                return StepType.Route;
            }
        }

        public override bool HasMultipleOutStep
        {
            get
            {
                return true;
            }
        }

        [ObjectElement(LocalName = "Connection", IsMultiple = true,
            ObjectType = typeof(ConnectionConfig))]
        public IEnumerable<ConnectionConfig> Connections
        {
            get
            {
                return fConnections;
            }
        }

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

        [SimpleAttribute]
        public FillContentMode FillMode { get; internal set; }

        public void AddConnectionConfig(ConnectionConfig connection)
        {
            fConnections.Add(connection);
        }

        internal override Step CreateStep(Workflow workflow)
        {
            return new RouteStep(workflow, this);
        }
    }
}