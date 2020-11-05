using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RawString : IRawString
    {
        public RawString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                value = string.Empty;

            Value = value;
        }

        public string Value { get; }

        public void WriteTo(TextWriter writer)
        {
            TkDebug.AssertArgumentNull(writer, nameof(writer), this);

            writer.Write(Value);
        }
    }
}