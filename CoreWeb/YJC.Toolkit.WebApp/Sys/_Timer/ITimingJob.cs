using System;

namespace YJC.Toolkit.Sys
{
    public interface ITimingJob
    {
        TimeSpan Interval { get; }

        void Process();
    }
}