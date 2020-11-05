using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public class TimingJobPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_TimingJob";
        private const string DESCRIPTION = "定时任务插件工厂";

        public TimingJobPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

        internal void FillTimingJobs(List<TimingJob> jobs)
        {
            EnumableCodePlugIn((regName, type, attr) =>
            {
                ITimingJob job = CreateInstance<ITimingJob>(regName);
                TimingJob timingJob = new TimingJob(job);
                jobs.Add(timingJob);
            });
        }
    }
}