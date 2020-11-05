using System;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Serializable]
    public sealed class NoActorException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the NoActorException class.
        /// </summary>
        public NoActorException(StepConfig stepConfig, ErrorConfig errorConfig)
            : base(stepConfig, errorConfig)
        {
        }
        public NoActorException(StepConfig stepConfig, ErrorConfig errorConfig, Exception innerException)
            : base(stepConfig, errorConfig, "没有找到下一步骤的操作者", innerException)
        {

        }

        internal override MistakeReason Reason
        {
            get
            {
                return MistakeReason.NoActor;
            }
        }
    }
}
