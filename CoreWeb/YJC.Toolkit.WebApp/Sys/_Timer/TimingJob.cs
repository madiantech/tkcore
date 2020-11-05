using System;

namespace YJC.Toolkit.Sys
{
    internal class TimingJob
    {
        private readonly TimeSpan fInterval;
        private readonly Action fAction;
        private readonly ITimingJob fJob;

        public TimingJob(ITimingJob job)
        {
            fJob = job;
            fInterval = job.Interval;
            fAction = job.Process;
            NextCalcTime = DateTime.Now + fInterval;
            Status = JobStatus.Idle;
        }

        public JobStatus Status { get; private set; }

        public DateTime NextCalcTime { get; private set; }

        public bool CanProcess(DateTime current)
        {
            return Status == JobStatus.Idle && NextCalcTime < current;
        }

        public void Process()
        {
            if (Status == JobStatus.Running)
                return;

            TkDebug.ThrowIfNoGlobalVariable();
            TkTrace.LogInfo($"'{fJob}'准备启动-{DateTime.Now}");
            Status = JobStatus.Running;
            BaseGlobalVariable.Current.BeginInvoke(EndProcess, fAction, null, null);
        }

        private void EndProcess(IAsyncResult ar)
        {
            Status = JobStatus.Idle;
            NextCalcTime = DateTime.Now + fInterval;
            TkTrace.LogInfo($"'{fJob}'停止工作-{DateTime.Now}");
        }
    }
}