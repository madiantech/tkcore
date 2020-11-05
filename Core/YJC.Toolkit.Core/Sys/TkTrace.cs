namespace YJC.Toolkit.Sys
{
    public static class TkTrace
    {
        public static void LogError(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            //TkDebug.ThrowIfNoGlobalVariable();
            BaseGlobalVariable.Current?.Trace?.LogError(message);
        }

        public static void LogInfo(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            //TkDebug.ThrowIfNoGlobalVariable();
            BaseGlobalVariable.Current?.Trace?.LogInfo(message);
        }

        public static void LogWarning(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            //TkDebug.ThrowIfNoGlobalVariable();
            BaseGlobalVariable.Current?.Trace?.LogWarning(message);
        }
    }
}