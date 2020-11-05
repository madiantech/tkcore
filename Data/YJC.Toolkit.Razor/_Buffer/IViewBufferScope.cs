using System.IO;

namespace YJC.Toolkit.Razor
{
    internal interface IViewBufferScope
    {
        ViewBufferValue[] GetPage(int pageSize);

        void ReturnSegment(ViewBufferValue[] segment);

        PagedBufferedTextWriter CreateWriter(TextWriter writer);
    }
}