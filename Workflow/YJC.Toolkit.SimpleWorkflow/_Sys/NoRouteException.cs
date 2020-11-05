using System;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Serializable]
    public sealed class NoRouteException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the NoRouteException class.
        /// </summary>
        public NoRouteException(StepConfig stepConfig, ErrorConfig errorConfig)
            : base(stepConfig, errorConfig)
        {

        }

        public NoRouteException(StepConfig stepConfig, ErrorConfig errorConfig, Exception innerException)
            : base(stepConfig, errorConfig, "没有找到下一步骤", innerException)
        {

        }

        internal override MistakeReason Reason
        {
            get
            {
                return MistakeReason.NoRoute;
            }
        }
    }
}
