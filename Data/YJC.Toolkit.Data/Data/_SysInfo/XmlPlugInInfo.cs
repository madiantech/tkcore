using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class XmlPlugInInfo : BasePlugInInfo
    {
        internal XmlPlugInInfo(string regName, Type type, BasePlugInAttribute attr)
            : base(regName, attr)
        {
            FileName = PlugInFactoryInfo.GetFileName(type);
            TypeName = type.ToString();
            var objAttr = attr.Convert<BaseObjectElementAttribute>();
            NamingRule = objAttr.NamingRule;
            NamespaceType = objAttr.NamespaceType;
            NamespaceUri = objAttr.NamespaceUri;
            UseConstructor = objAttr.UseConstructor;
        }

        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute]
        public string TypeName { get; private set; }

        [SimpleAttribute]
        public NamingRule NamingRule { get; private set; }

        [SimpleAttribute]
        public NamespaceType NamespaceType { get; private set; }

        [SimpleAttribute]
        public string NamespaceUri { get; private set; }

        [SimpleAttribute]
        public bool UseConstructor { get; private set; }
    }
}
