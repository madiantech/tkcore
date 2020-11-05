using System;
using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal sealed class FinishData : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the FinishData class.
        /// </summary>
        public FinishData(Workflow workflow)
        {
            Resolvers = new WorkflowResolvers(workflow.Context);
            WfHisResolver = new WorkflowInstHisResolver(workflow.Source);
            StepHisResolver = new StepInstHisResolver(workflow.Source);

            Resolvers.AddResolver(workflow.WorkflowResolver);
            Resolvers.AddResolver(workflow.StepResolver);
            Resolvers.AddResolver(WfHisResolver);
            Resolvers.AddResolver(StepHisResolver);
        }

        public DataRow WfHistoryRow { get; set; }

        public WorkflowContent Content { get; set; }

        public WorkflowResolvers Resolvers { get; private set; }

        public TableResolver WfHisResolver { get; private set; }

        public TableResolver StepHisResolver { get; private set; }

        #region IDisposable 成员

        public void Dispose()
        {
            Resolvers.Dispose();
            if (Content != null)
                Content.Dispose();
        }

        #endregion IDisposable 成员
    }
}