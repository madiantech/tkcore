using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class NameModelAttribute : Attribute
    {
        private string fLocalName;
        private string fNamespace;
        private NamespaceType fNamespaceType;

        public NameModelAttribute(string modelName)
        {
            ModelName = modelName;
        }

        public string ModelName { get; private set; }

        public NamingRule NamingRule { get; set; }

        public string LocalName
        {
            get
            {
                return fLocalName;
            }
            set
            {
                TkDebug.AssertNotNullOrEmpty(value, "value", this);
                TkDebug.Assert(value.IndexOf(':') == -1, string.Format(ObjectUtil.SysCulture,
                    "{0}是带有Namespace的Xml名称，请使用它的LocalName给属性赋值", value), this);
                fLocalName = value;
            }
        }

        public NamespaceType NamespaceType
        {
            get
            {
                return fNamespaceType;
            }
            set
            {
                if (value == NamespaceType.Toolkit)
                    fNamespace = ToolkitConst.NAMESPACE_URL;
                fNamespaceType = value;
            }
        }

        public string NamespaceUri
        {
            get
            {
                return fNamespace;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    fNamespaceType = NamespaceType.None;
                else if (value == ToolkitConst.NAMESPACE_URL)
                    fNamespaceType = NamespaceType.Toolkit;
                else
                    fNamespaceType = NamespaceType.Namespace;
                fNamespace = value;
            }
        }
    }
}
