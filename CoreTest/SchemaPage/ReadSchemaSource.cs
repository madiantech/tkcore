using System;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using System.Linq;
using Toolkit.SchemaSuite.Data;
using Toolkit.SchemaSuite.Schema;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    [Source]
    internal class ReadSchemaSource : ISource
    {
        public OutputData DoAction(IInputData input)
        {
            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, @"testSchema/100010/1.0/100010.xsd");
            SchemaTree tree = new SchemaTree(fileName, string.Empty, "恢复权利请求书");
            //string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, @"testSchema/100016/1.0/100016.xsd");
            //SchemaTree tree = new SchemaTree(fileName, string.Empty, "著录项目变更申报书");
            tree.Judge();
            PageData pageData = new PageData();
            var table = tree.CreateMetaData(pageData);
            dynamic obj = new ExpandoObject();
            obj.MetaData = table;
            obj.PageData = pageData;

            //fileName = Path.Combine(BaseAppSetting.Current.XmlPath, @"testSchema/100010/1.0/100010.xml");
            //T100010Xml xml = T100010Xml.ReadXmlFromFile(fileName);
            //string newXml = xml.WriteXml(WriteSettings.Default, T100010Xml.ROOT);
            //string json = xml.WriteJson();

            return OutputData.CreateObject(obj);
        }
    }
}