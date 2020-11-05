using System.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public interface IWorkflowConfig
    {
        DataRow WorkflowRow { get; }

        WorkflowConfig Config { get; }

        StepConfig CurrentStepConfig { get; }
    }
}
