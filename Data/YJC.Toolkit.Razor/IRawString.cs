using System.IO;

namespace YJC.Toolkit.Razor
{
    public interface IRawString
    {
        void WriteTo(TextWriter writer);
    }
}