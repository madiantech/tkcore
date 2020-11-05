using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppStarting(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
            string path = Path.Combine(appsetting.XmlPath, "razor.xml");
            if (File.Exists(path))
            {
                RazorConfigXml xml = new RazorConfigXml();
                xml.ReadXmlFromFile(path);
                xml.SetConfiguration(RazorConfiguration.Current);
            }
        }

        public void AppStarted(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
        }

        public void AppEnd(object application)
        {
        }

        #endregion
    }
}
