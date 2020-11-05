using System.Reflection;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public static class ToolkitConst
    {
        public const string DEBUG = "DEBUG";

        public static readonly string TOOLKIT = "Toolkit";

        public static readonly string ROOT_NODE_NAME = "Toolkit";

        public const string VERSION = "version";

        public const string TK_NS = "tk";

        public const string NAMESPACE_URL = "http://www.qdocuments.net";

        public const string DATE_FMT_STR = "yyyy-MM-dd";

        public const string DATETIME_FMT_STR = "yyyy-MM-dd HH:mm:ss";

        public const char QUOTE_CHAR = '"';

        public const string TOOLKIT_XML_SKELETON = "<tk:Toolkit xmlns:tk='http://www.qdocuments.net'>{0}</tk:Toolkit>";

        public static readonly Assembly TOOLKIT_CORE_ASSEMBLY = Assembly.GetExecutingAssembly();

        public static readonly AssemblyName TOOLKIT_CORE_NAME =
            new AssemblyName(TOOLKIT_CORE_ASSEMBLY.FullName);

        public static readonly Encoding UTF8 = new UTF8Encoding(false);
    }
}