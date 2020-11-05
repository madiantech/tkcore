using Microsoft.Extensions.Logging;

namespace YJC.Toolkit.Sys
{
    internal class ToolkitWebTrace : ITrace
    {
        private readonly ILogger<ToolkitService> fLogger;

        public ToolkitWebTrace(in ILogger<ToolkitService> logger)
        {
            fLogger = logger;
        }

        public void LogError(string message)
        {
            fLogger.LogError(message);
        }

        public void LogInfo(string message)
        {
            fLogger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            fLogger.LogWarning(message);
        }
    }
}