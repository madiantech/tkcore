namespace YJC.Toolkit.Razor
{
    public interface IRazorPath
    {
        string LocalPath { get; }

        string LayoutPath { get; }

        string LayoutFile { get; }

        void ClearLayoutFile();
    }
}
