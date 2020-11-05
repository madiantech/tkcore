using System.IO;
using System.Xml;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class XmlPostDataSetCreator : BasePostDataSetObjectCreator
    {
        public static readonly IPostObjectCreator Creator = new XmlPostDataSetCreator();

        private XmlPostDataSetCreator()
        {
        }

        protected override XmlReader CreateReader(Stream stream)
        {
            return XmlReader.Create(stream, new XmlReaderSettings { CloseInput = true });
        }
    }
}
