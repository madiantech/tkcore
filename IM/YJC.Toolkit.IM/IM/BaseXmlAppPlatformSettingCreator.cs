using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public abstract class BaseXmlAppPlatformSettingCreator<T> : BaseAppPlatformSettingCreator<T>
    {
        protected BaseXmlAppPlatformSettingCreator(string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            FileName = fileName;
        }

        public string FileName { get; private set; }

        protected override void ReadTokenXml(AppAccessTokenXml tokenXml)
        {
            if (File.Exists(FileName))
                tokenXml.ReadXmlFromFile(FileName);
        }

        protected override void SaveTokenXml(AppAccessTokenXml tokenXml)
        {
            FileUtil.VerifySaveFile(FileName, tokenXml.WriteXml(), ToolkitConst.UTF8);
        }
    }
}