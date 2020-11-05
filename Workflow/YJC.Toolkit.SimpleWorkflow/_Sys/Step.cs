using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public abstract class Step
    {
        private readonly Workflow fWorkflow;

        protected Step(Workflow workflow, StepConfig config)
        {
            fWorkflow = workflow;
            Config = config;
        }

        public StepConfig Config { get; private set; }

        public Workflow Workflow
        {
            get
            {
                return fWorkflow;
            }
        }

        public DataRow WorkflowRow
        {
            get
            {
                return fWorkflow.WorkflowRow;
            }
        }

        public IDbDataSource Source
        {
            get
            {
                return fWorkflow.Source;
            }
        }

        internal WorkflowInstResolver WorkflowResolver
        {
            get
            {
                return fWorkflow.WorkflowResolver;
            }
        }

        internal StepInstResolver StepResolver
        {
            get
            {
                return fWorkflow.StepResolver;
            }
        }

        public void ClearDataSet()
        {
            foreach (DataTable table in Source.DataSet.Tables)
            {
                if (table.TableName != "WF_WORKFLOW_INST")
                    table.Rows.Clear();
            }
        }

        protected abstract bool Execute();

        protected abstract void Send(StepConfig nextStep);

        internal abstract State GetState(StepState state);

        public bool ExecuteStep()
        {
            bool result = Execute();
            ClearDataSet();
            return result;
        }

        public bool SendStep(StepConfig nextStep)
        {
            Send(nextStep);
            ClearDataSet();
            return true;
        }
    }
}