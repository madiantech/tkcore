using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Serializable]
    public abstract class WorkflowException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WorkflowException class.
        /// </summary>
        protected WorkflowException(StepConfig stepConfig, ErrorConfig errorConfig)
        {
            StepConfig = stepConfig;
            ErrorConfig = errorConfig;
        }

        protected WorkflowException(StepConfig stepConfig, ErrorConfig errorConfig,
            string message, Exception innerException)
            : base(message, innerException)
        {
            StepConfig = stepConfig;
            ErrorConfig = errorConfig;
        }

        internal abstract MistakeReason Reason { get; }

        public StepConfig StepConfig { get; private set; }

        public ErrorConfig ErrorConfig { get; private set; }

        [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
