using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ProcessXmlConfig(NamespaceType = NamespaceType.Toolkit, RegName = "XmlFile", Author = "YJC",
        CreateDate = "2017-10-23", Description = "根据Xml文件的路径实例化相应的ProcessXml")]
    internal class ProcessXmlFileConfig : IConfigCreator<ProcessXml>
    {
        [SimpleAttribute]
        public string FileName { get; private set; }

        public ProcessXml CreateObject(params object[] args)
        {
            TkDebug.ThrowIfNoAppSetting();
            string path = Path.Combine(WebAppSetting.Current.XmlPath, "Workflow", FileName);

            ProcessXml config = new ProcessXml();
            config.ReadXmlFromFile(path);
            return config;
        }
    }
}