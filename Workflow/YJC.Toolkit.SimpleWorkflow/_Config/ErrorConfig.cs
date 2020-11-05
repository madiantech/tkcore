using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ErrorConfig
    {
        private const int DEFAULT_TIMES = 10;
        private const string DEFAULT_INTERVAL = "1.00:00:00";

        public ErrorConfig()
        {
            ProcessType = ErrorProcessType.Retry;
            RetryTimes = DEFAULT_TIMES;
            Interval = TimeSpan.Parse(DEFAULT_INTERVAL);
        }

        [SimpleAttribute(DefaultValue = ErrorProcessType.Retry)]
        public ErrorProcessType ProcessType { get; internal set; }

        [SimpleAttribute(DefaultValue = DEFAULT_TIMES)]
        public int RetryTimes { get; internal set; }

        [SimpleAttribute(DefaultValue = DEFAULT_INTERVAL)]
        public TimeSpan Interval { get; internal set; }
    }
}