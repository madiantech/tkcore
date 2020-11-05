using System.Diagnostics;

namespace YJC.Toolkit.Sys
{
    internal class DefaultTrace : ITrace
    {
        public static readonly ITrace Instance = new DefaultTrace();

        private DefaultTrace()
        {
        }

        public void LogError(string message)
        {
            Trace.TraceError(message);
        }

        public void LogInfo(string message)
        {
            Trace.TraceInformation(message);
        }

        public void LogWarning(string message)
        {
            Trace.TraceWarning(message);
        }
    }
}