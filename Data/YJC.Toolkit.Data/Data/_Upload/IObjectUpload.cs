
namespace YJC.Toolkit.Data
{
    public interface IObjectUpload
    {
        string FileName { get; }

        string ContentType { get; }

        string ServerPath { get; }

        int FileSize { get; }

        string WebPath { get; }
    }
}
