using System.IO;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleCreator(Author = "YJC", CreateDate = "2015-09-28",
        Description = "通过Xml\\Module下的文件来创建对应的Module")]
    [AlwaysCache, InstancePlugIn]
    internal class XmlModuleCreator : IModuleCreator
    {
        public static readonly IModuleCreator Instance = new XmlModuleCreator();

        private XmlModuleCreator()
        {
        }

        #region IModuleCreator 成员

        public IModule Create(string source)
        {
            TkDebug.AssertArgumentNullOrEmpty(source, "source", this);
            TkDebug.ThrowIfNoAppSetting();

            string path = Path.Combine(BaseAppSetting.Current.XmlPath, "Module", source + ".xml");
            Tk5ModuleXml xml = new Tk5ModuleXml();
            xml.ReadXmlFromFile(path);
            return xml;
        }

        #endregion
    }
}
