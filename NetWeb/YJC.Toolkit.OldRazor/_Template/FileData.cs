using System;
using System.IO;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class FileData
    {
        public FileData(Type baseType, string fileName, string layoutFile,
            Encoding encoding, object configurationData)
        {
            BaseType = baseType ?? typeof(RazorFileTemplate);
            if (configurationData == null || !(configurationData is IRazorPath))
            {
                string basePath = Path.GetDirectoryName(fileName);
                ConfigurationData = new RazorPath(basePath, layoutFile);
            }
            else
                ConfigurationData = configurationData;
            if (encoding == null)
                encoding = Encoding.UTF8;
            CacheName = fileName.ToLower(ObjectUtil.SysCulture);
            RazorTemplate = File.ReadAllText(fileName, encoding);
        }

        public Type BaseType { get; private set; }

        public string RazorTemplate { get; private set; }

        public string CacheName { get; private set; }

        public object ConfigurationData { get; private set; }
    }
}
