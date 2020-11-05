using System.IO;
using System.Text;

namespace YJC.Toolkit.Sys.Json
{
    internal static class StringUtil
    {
        public static bool IsWhiteSpace(string str)
        {
            TkDebug.AssertArgumentNull(str, "str", null);

            if (str.Length == 0)
                return false;

            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsWhiteSpace(str[i]))
                    return false;
            }

            return true;
        }

        public static StringWriter CreateStringWriter(int capacity)
        {
            StringBuilder sb = new StringBuilder(capacity);
            StringWriter sw = new StringWriter(sb, ObjectUtil.SysCulture);

            return sw;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 48);
            }
            return (char)((n - 10) + 97);
        }

        public static string AsUnicode(this char ch)
        {
            char h1 = IntToHex((ch >> 12) & '\x000f');
            char h2 = IntToHex((ch >> 8) & '\x000f');
            char h3 = IntToHex((ch >> 4) & '\x000f');
            char h4 = IntToHex(ch & '\x000f');

            return new string(new[] { '\\', 'u', h1, h2, h3, h4 });
        }
    }
}
