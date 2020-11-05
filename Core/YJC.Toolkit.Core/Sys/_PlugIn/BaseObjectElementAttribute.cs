using System;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseObjectElementAttribute : BasePlugInAttribute
    {
        private string fNamespace;
        private NamespaceType fNamespaceType;

        protected BaseObjectElementAttribute()
        {
            Suffix = "Config";
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

        public NamingRule NamingRule { get; set; }

        public bool UseConstructor { get; set; }

        public string Version { get; set; }

        internal ObjectElementAttribute ConvertTo(string localName, Type objectType,
            DynamicElementAttribute dynamic)
        {
            ObjectElementAttribute attr = new ObjectElementAttribute(NamespaceType)
            {
                LocalName = localName,
                ObjectType = objectType,
                UseConstructor = UseConstructor,
                Order = dynamic.Order,
                IsMultiple = dynamic.IsMultiple,
                CollectionType = dynamic.CollectionType
            };
            if (NamespaceType == NamespaceType.Namespace)
                attr.NamespaceUri = NamespaceUri;

            return attr;
        }
    }
}