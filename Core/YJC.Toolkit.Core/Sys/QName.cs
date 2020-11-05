using System.Xml;

namespace YJC.Toolkit.Sys
{
    [TkTypeConverter(typeof(QNameConverter))]
    public sealed class QName
    {
        public static readonly QName Toolkit = new QName(ToolkitConst.NAMESPACE_URL,
            ToolkitConst.ROOT_NODE_NAME);

        public static readonly QName ToolkitNoNS = new QName(null, ToolkitConst.ROOT_NODE_NAME);

        private readonly int fHashCode;

        /// <summary>
        /// Initializes a new instance of the QName class.
        /// </summary>
        private QName(string @namespace, string localName)
        {
            LocalName = localName;
            if (string.IsNullOrEmpty(@namespace))
            {
                Namespace = string.Empty;
                HasNamespace = false;
                fHashCode = LocalName.GetHashCode();
            }
            else
            {
                Namespace = @namespace;
                HasNamespace = true;
                fHashCode = Namespace.GetHashCode() ^ LocalName.GetHashCode();
            }
        }

        public bool HasNamespace { get; private set; }

        public string Namespace { get; private set; }

        public string LocalName { get; private set; }

        public bool IgnoreNamespace { get; set; }

        public override bool Equals(object obj)
        {
            if (object.Equals(obj, null))
                return false;
            QName other = obj as QName;
            if (object.Equals(other, null))
                return false;

            if (IgnoreNamespace || other.IgnoreNamespace)
                return LocalName == other.LocalName;

            if (HasNamespace)
            {
                if (other.HasNamespace)
                    return Namespace == other.Namespace && LocalName == other.LocalName;
                else
                    return false;
            }
            else
            {
                if (other.HasNamespace)
                    return false;
                else
                    return LocalName == other.LocalName;
            }
        }

        public override int GetHashCode()
        {
            return fHashCode;
        }

        public override string ToString()
        {
            return HasNamespace ? string.Format(ObjectUtil.SysCulture,
                "{{{0}}}{1}", Namespace, LocalName) : LocalName;
        }

        public static bool operator ==(QName left, QName right)
        {
            return ObjectUtil.Equals(left, right);
        }

        public static bool operator !=(QName left, QName right)
        {
            return !ObjectUtil.Equals(left, right);
        }

        public static implicit operator QName(string expandedName)
        {
            return GetFullName(expandedName);
        }

        public static QName Get(string localName)
        {
            TkDebug.AssertArgumentNullOrEmpty(localName, "localName", null);

            return new QName(null, localName);
        }

        public static QName Get(string localName, string @namespace)
        {
            TkDebug.AssertArgumentNullOrEmpty(localName, "localName", null);

            return new QName(@namespace, localName);
        }

        public static QName GetFullName(string expandedName)
        {
            TkDebug.AssertArgumentNullOrEmpty(expandedName, "expandedName", null);

            if (expandedName[0] != '{')
                return Get(expandedName);

            int num = expandedName.LastIndexOf('}');
            if ((num <= 1) || (num == (expandedName.Length - 1)))
            {
                TkDebug.ThrowToolkitException(expandedName + "格式错误", null);
            }
            string @namespace = expandedName.Substring(1, num);
            string localName = expandedName.Substring(num + 1);
            return new QName(@namespace, localName);
        }

        public static QName Get(XmlReader reader)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            return new QName(reader.NamespaceURI, reader.LocalName);
        }
    }
}