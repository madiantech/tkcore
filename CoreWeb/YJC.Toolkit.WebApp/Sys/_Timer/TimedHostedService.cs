using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YJC.Toolkit.Sys
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private static readonly TimeSpan StartDelay = TimeSpan.FromMinutes(5);

        private Timer fTimer;

        public TimedHostedService(IToolkitService svc)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                fTimer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            TkDebug.ThrowIfNoAppSetting();
            var interval = WebAppSetting.WebCurrent.TimingInterval;
            fTimer = new Timer(DoWork, null, StartDelay, interval);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            fTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var jobs = WebGlobalVariable.WebCurrent.Jobs;
            if (jobs.Count == 0)
                return;
            DateTime current = DateTime.Now;

            foreach (var job in jobs)
            {
                if (job.CanProcess(current))
                    job.Process();
            }
        }
    }
}