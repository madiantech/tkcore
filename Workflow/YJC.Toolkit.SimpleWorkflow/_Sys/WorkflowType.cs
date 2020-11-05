using System;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Flags]
    public enum WorkflowType
    {
        None = 0,
        Parent = 1,
        Child = 2,
        Both = 3
    }
}
