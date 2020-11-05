using System.Xml.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Xml
{
    public static class XmlExtension
    {
        public static XName ToXName(this QName name)
        {
            if (name == null)
                return null;

            return name.HasNamespace ? XName.Get(name.LocalName, name.Namespace)
                : XName.Get(name.LocalName);
        }

        public static QName ToQName(this XName name)
        {
            if (name == null)
                return null;

            return QName.Get(name.LocalName, name.NamespaceName);
        }
    }
}
