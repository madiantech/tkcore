namespace YJC.Toolkit.Sys
{
    public interface ITrace
    {
        void LogError(string message);

        void LogInfo(string message);

        void LogWarning(string message);
    }
}