using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class PlugInManagerData
    {
        internal PlugInManagerData()
        {
            TkDebug.ThrowIfNoGlobalVariable();
            PlugInFactoryManager manager = BaseGlobalVariable.Current.FactoryManager;

            CodeFactories = CreateList(manager.CodeFactories);
            XmlConfigs = CreateList(manager.XmlConfigs);
        }

        [ObjectElement(IsMultiple = true, CollectionType = typeof(List<PlugInFactoryInfo>))]
        public IEnumerable<PlugInFactoryInfo> CodeFactories { get; private set; }

        [ObjectElement(IsMultiple = true, CollectionType = typeof(List<PlugInFactoryInfo>))]
        public IEnumerable<PlugInFactoryInfo> XmlConfigs { get; private set; }

        private List<PlugInFactoryInfo> CreateList(IEnumerable<BasePlugInFactory> list)
        {
            List<PlugInFactoryInfo> result = new List<PlugInFactoryInfo>(list.Count());
            foreach (var item in list)
            {
                PlugInFactoryInfo info = new PlugInFactoryInfo(item);
                result.Add(info);
            }

            return result;
        }
    }
}
