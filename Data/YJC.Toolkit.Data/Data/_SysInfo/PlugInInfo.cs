using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class PlugInInfo : BasePlugInInfo
    {
        internal PlugInInfo()
        {
        }

        internal PlugInInfo(string regName, Type type, BasePlugInAttribute attr)
            : base(regName, attr)
        {
            FileName = PlugInFactoryInfo.GetFileName(type);
            TypeName = type.ToString();
            PlugType = PlugInType.Code;
        }

        internal PlugInInfo(string regName, BasePlugInAttribute attr, object obj)
            : base(regName, attr)
        {
            Type type = obj.GetType();
            FileName = PlugInFactoryInfo.GetFileName(type);
            TypeName = type.ToString();
            PlugType = PlugInType.Instance;
        }

        internal PlugInInfo(IXmlPlugInItem item, string fileName, Type baseType, BasePlugInAttribute attr)
            : base(item.RegName, attr)
        {
            FileName = fileName;
            TypeName = baseType.ToString();
            BaseClass = item.BaseClass;
            PlugType = PlugInType.Xml;
        }

        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute]
        public string TypeName { get; private set; }

        [SimpleAttribute]
        public PlugInType PlugType { get; private set; }

        [SimpleAttribute]
        public string BaseClass { get; private set; }
    }
}
