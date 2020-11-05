using System.Linq;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    public abstract class NamedAttribute : BaseObjectAttribute
    {
        private string fNamespace;
        private NamespaceType fNamespaceType;
        private string fLocalName;

        protected NamedAttribute()
        {
        }

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

        public NamingRule NamingRule { get; set; }

        internal QName GetQName(string name)
        {
            switch (NamespaceType)
            {
                case NamespaceType.None:
                    return name;

                default:
                    return QName.Get(name, NamespaceUri);
            }
        }

        internal void SetNameMode(string modelName, PropertyInfo propertyInfo)
        {
            if (string.IsNullOrEmpty(modelName))
                return;

            object[] attrs = propertyInfo.GetCustomAttributes(typeof(NameModelAttribute), false);
            if (attrs.Length == 0)
                return;

            var modeAttr = (from item in attrs
                            let attr = (NameModelAttribute)item
                            where attr.ModelName == modelName
                            select attr).FirstOrDefault();
            if (modeAttr == null)
                return;

            if (!string.IsNullOrEmpty(modeAttr.LocalName))
                LocalName = modeAttr.LocalName;
            else
                fLocalName = null;
            NamespaceUri = modeAttr.NamespaceUri;
            NamespaceType = modeAttr.NamespaceType;
            NamingRule = modeAttr.NamingRule;
        }

        public void Assign(QName name)
        {
            if (name == null)
                return;

            if (name.HasNamespace)
            {
                NamespaceUri = name.Namespace;
                LocalName = name.LocalName;
            }
            else
                LocalName = name.LocalName;
        }
    }
}