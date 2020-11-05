using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ContentXmlConfig(RegName = "XmlFile", NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2018-01-31", Description = "根据Xml文件的路径实例化相应的ContentXml")]
    internal class ContentXmlPlugInConfig : IConfigCreator<ContentXml>
    {
        #region IConfigCreator<ContentXml> 成员

        public ContentXml CreateObject(params object[] args)
        {
            TkDebug.ThrowIfNoAppSetting();
            string path = Path.Combine(BaseAppSetting.Current.XmlPath, "Workflow", FileName);

            ContentXml xml = new ContentXml();
            xml.ReadXmlFromFile(path);
            return xml;
        }

        #endregion IConfigCreator<ContentXml> 成员

        [SimpleAttribute(Required = true)]
        public string FileName { get; private set; }
    }
}