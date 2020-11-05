using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toolkit.SchemaSuite.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    [Source]
    internal class ReadSchemaDataSource : ISource
    {
        public OutputData DoAction(IInputData input)
        {
            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, @"testSchema/100010/1.0/100010.xml");
            T100010Xml xml = T100010Xml.ReadXmlFromFile(fileName);

            return OutputData.CreateToolkitObject(xml);
        }
    }
}