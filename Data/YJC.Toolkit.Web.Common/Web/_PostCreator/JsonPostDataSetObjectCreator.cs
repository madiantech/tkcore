using System.IO;
using System.Xml;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    public class JsonPostDataSetObjectCreator : BasePostDataSetObjectCreator
    {
        public static readonly IPostObjectCreator Creator = new JsonPostDataSetObjectCreator();

        private JsonPostDataSetObjectCreator()
        {
        }

        protected override XmlReader CreateReader(Stream stream)
        {
            return new XmlJsonReader(stream);
        }
    }
}
