using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    internal class AliyunOSSInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppEnd(object application)
        {
        }

        public void AppStarted(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
            string fileName = Path.Combine(appsetting.XmlPath, "AliyunOSS.xml");
            if (File.Exists(fileName))
            {
                AliyunOSSConfigXml xml = new AliyunOSSConfigXml();
                xml.ReadXmlFromFile(fileName);
                AliyunOSSSetting.CreateAliyunOSSSetting(xml);
            }
        }

        public void AppStarting(object application, BaseAppSetting appsetting, BaseGlobalVariable globalVariable)
        {
        }

        #endregion IInitialization 成员
    }
}