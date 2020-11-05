using System.IO;
using System.Xml.Linq;

namespace YJC.Toolkit.Sys
{
    internal sealed class XElementData
    {
        public XElementData()
        {
        }

        public XElementData(XElementData data, XElement element)
        {
            Root = data.Root;
            Current = element;
            Stream = data.Stream;
        }

        public XDocument Root { get; set; }

        public XElement Current { get; set; }

        public Stream Stream { get; set; }

        public static XElement GetCurrent(object reader)
        {
            return reader.Convert<XElementData>().Current;
        }
    }
}
