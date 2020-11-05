using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class PlugInFactoryData
    {
        internal PlugInFactoryData(BasePlugInFactory factory)
        {
            FactoryInfo = new PlugInFactoryInfo(factory);
            List<PlugInInfo> list = new List<PlugInInfo>();
            factory.EnumableCodePlugIn((regName, type, attr) =>
                list.Add(new PlugInInfo(regName, type, attr)));
            BaseInstancePlugInFactory instanceFactory = factory as BaseInstancePlugInFactory;
            if (instanceFactory != null)
                instanceFactory.EnumableInstancePlugIn((regName, obj, attr) =>
                    list.Add(new PlugInInfo(regName, attr, obj)));
            BaseXmlPlugInFactory xmlFactory = factory as BaseXmlPlugInFactory;
            if (xmlFactory != null)
                xmlFactory.EnumableXmlPlugIn((item, fileName, type, attr) =>
                    list.Add(new PlugInInfo(item, fileName, type, attr)));
            PlugIns = list;
        }

        [ObjectElement]
        public PlugInFactoryInfo FactoryInfo { get; private set; }

        [ObjectElement(IsMultiple = true, CollectionType = typeof(List<PlugInInfo>), UseConstructor = true)]
        public IEnumerable<PlugInInfo> PlugIns { get; private set; }
    }
}
