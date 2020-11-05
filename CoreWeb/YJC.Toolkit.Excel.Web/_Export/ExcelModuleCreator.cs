using System.IO;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleCreator(Author = "YJC", CreateDate = "2015-09-28",
        Description = "创建导出Excel的Module，首先建立在Tk5ModuleXml的基础上")]
    internal class ExcelModuleCreator : IModuleCreator
    {
        #region IModuleCreator 成员

        public IModule Create(string source)
        {
            TkDebug.AssertArgumentNullOrEmpty(source, "source", this);
            TkDebug.ThrowIfNoAppSetting();

            string path = Path.Combine(BaseAppSetting.Current.XmlPath, "Module", source + ".xml");
            Tk5ModuleXml xml = new Tk5ModuleXml();
            xml.ReadXmlFromFile(path);

            return new ExportExcelModule(xml);
        }

        #endregion
    }
}
