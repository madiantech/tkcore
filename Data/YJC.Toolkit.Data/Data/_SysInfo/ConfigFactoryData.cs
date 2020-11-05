using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ConfigFactoryData
    {
        internal ConfigFactoryData(BaseXmlConfigFactory factory)
        {
            FactoryInfo = new PlugInFactoryInfo(factory);
            List<XmlPlugInInfo> list = new List<XmlPlugInInfo>();
            factory.EnumableCodePlugIn((regName, type, attr) =>
                list.Add(new XmlPlugInInfo(regName, type, attr)));
            PlugIns = list;
        }

        [ObjectElement]
        public PlugInFactoryInfo FactoryInfo { get; private set; }

        [ObjectElement(IsMultiple = true, CollectionType = typeof(List<PlugInInfo>), UseConstructor = true)]
        public IEnumerable<XmlPlugInInfo> PlugIns { get; private set; }
    }
}
