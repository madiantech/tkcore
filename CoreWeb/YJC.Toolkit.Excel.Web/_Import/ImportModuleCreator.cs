using System.IO;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Excel
{
    [ModuleCreator(Author = "YJC", CreateDate = "2016-08-08",
        Description = "通过Xml\\Module下的Import文件创建导入的Module")]
    [AlwaysCache, InstancePlugIn]
    internal class ImportModuleCreator : IModuleCreator
    {
        public static readonly IModuleCreator Instance = new ImportModuleCreator();

        private ImportModuleCreator()
        {
        }

        #region IModuleCreator 成员

        public IModule Create(string source)
        {
            string path = Path.Combine(BaseAppSetting.Current.XmlPath, "Module", source + ".xml");
            ImportConfigXml xml = new ImportConfigXml();
            xml.ReadXmlFromFile(path);

            return new ImportExcelModule(xml);
        }

        #endregion
    }
}
