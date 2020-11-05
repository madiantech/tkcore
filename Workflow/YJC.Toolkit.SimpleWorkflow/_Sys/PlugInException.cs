using System;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Serializable]
    public sealed class PlugInException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the PlugInException class.
        /// </summary>
        public PlugInException(StepConfig stepConfig, ErrorConfig errorConfig, Exception innerException)
            : base(stepConfig, errorConfig, "插件计算错误", innerException)
        {
        }

        internal override MistakeReason Reason
        {
            get
            {
                return MistakeReason.PlugInError;
            }
        }
    }
}
