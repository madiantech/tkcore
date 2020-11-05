using System;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public static class StringUtil
    {
        private readonly static Encoding GB_ENCODING = GetSafeEncoding();
        private static readonly string[] XML_ESCAPE = new string[] { "&amp;", "&lt;", "&gt;", "&apos;" };
        private static readonly string[] HTML_ATTRS = new string[] { "&amp;", "&lt;", "&quot;" };
        private static readonly string[] HTML = new string[] { "&amp;", "&lt;" };

        /// <summary>
        /// 将字符串中的指定字符替换成转义字符串，算法效率相对高效
        /// </summary>
        /// <param name="source">需要转义的字符串</param>
        /// <param name="replaceString">需要替换字符串数组</param>
        /// <param name="escapeFunc">判断是否需要转义字符，返回-1无需转义，
        /// 返回其他数字将去数组replaceString查找需要替换的字符串</param>
        /// <returns></returns>
        public static string EscapeString(string source, string[] replaceString, Func<char, int> escapeFunc)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            TkDebug.AssertArgumentNull(escapeFunc, "escapeFunc", null);
            TkDebug.AssertEnumerableArgumentNull<string>(replaceString, "replaceString", null);

            StringBuilder cachedStringBuilder = null;
            int start = 0;
            int pos = 0;
            int count = source.Length;
            for (int i = 0; i < count; ++i)
            {
                pos = escapeFunc(source[i]);
                if (pos < 0)
                    continue;
                if (cachedStringBuilder == null)
                    cachedStringBuilder = new StringBuilder();
                cachedStringBuilder.Append(source.Substring(start, i - start));
                TkDebug.Assert(pos < replaceString.Length, string.Format(ObjectUtil.SysCulture,
                    "参数escapeFunc返回的值有误，replaceString的长度为{0}，但是返回值却是{1}，越界了",
                    replaceString.Length, pos), null);
                cachedStringBuilder.Append(replaceString[pos]);
                start = i + 1;
            }
            if (start == 0)
                return source;
            else if (start < count)
                cachedStringBuilder.Append(source.Substring(start, count - start));
            string s = cachedStringBuilder.ToString();
            return s;
        }

        /// <summary>
        /// 转义Xml字符串
        /// </summary>
        /// <param name="source">需要转义的字符串</param>
        /// <returns>Xml的&amp;,&lt;,&gt;,&apos;将被转义</returns>
        public static string EscapeXmlString(string source)
        {
            return EscapeString(source, XML_ESCAPE, c =>
            {
                switch (c)
                {
                    case '&':
                        return 0;

                    case '<':
                        return 1;

                    case '>':
                        return 2;

                    case '\'':
                        return 3;

                    default:
                        return -1;
                }
            });
        }

        public static string EscapeHtmlAttribute(string attr)
        {
            return StringUtil.EscapeString(attr, HTML_ATTRS, c =>
            {
                switch (c)
                {
                    case '&':
                        return 0;

                    case '<':
                        return 1;

                    case '"':
                        return 2;

                    default:
                        return -1;
                }
            });
        }

        public static string EscapeHtml(string data)
        {
            return StringUtil.EscapeString(data, HTML, c =>
            {
                switch (c)
                {
                    case '&':
                        return 0;

                    case '<':
                        return 1;

                    default:
                        return -1;
                }
            });
        }

        private readonly static string[] SqlReplacements = new string[] { @"\\", @"\%", @"\_" };

        /// <summary>
        /// 转义Sql语句
        /// </summary>
        /// <param name="source">需要转义的字符串</param>
        /// <returns>Sql语句的通配符%，_以及\将被转义</returns>
        public static string EscapeSqlString(string source)
        {
            return EscapeString(source, SqlReplacements, c =>
            {
                switch (c)
                {
                    case '\\':
                        return 0;

                    case '%':
                        return 1;

                    case '_':
                        return 2;

                    default:
                        return -1;
                }
            });
        }

        private readonly static string[] AposReplacement = new string[] { "''" };

        /// <summary>
        /// 转义字符串中的单引号，在拼接SQL中可以有效防止Sql注入
        /// </summary>
        /// <param name="source">需要转义的字符串</param>
        /// <returns>将一个单引号转义成两个单引号</returns>
        public static string EscapeAposString(string source)
        {
            return EscapeString(source, AposReplacement, c =>
            {
                switch (c)
                {
                    case '\'':
                        return 0;

                    default:
                        return -1;
                }
            });
        }

        private static string GetPartName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            return Char.ToUpperInvariant(name[0]) + name.Substring(1).ToLower();
        }

        public static string GetNickName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return fieldName;

            string[] fieldArr = fieldName.Split('_');
            if (fieldArr.Length == 1)
                return GetPartName(fieldArr[0]);
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < fieldArr.Length; i++)
                builder.Append(GetPartName(fieldArr[i]));

            return builder.ToString();
        }

        public static string GetName(NamingRule rule, string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return fieldName;

            switch (rule)
            {
                case NamingRule.Pascal:
                    if (char.IsUpper(fieldName[0]))
                        return fieldName;
                    return char.ToUpper(fieldName[0], ObjectUtil.SysCulture)
                        + fieldName.Substring(1);

                case NamingRule.Camel:
                    if (char.IsLower(fieldName[0]))
                        return fieldName;
                    return char.ToLower(fieldName[0], ObjectUtil.SysCulture)
                        + fieldName.Substring(1);

                case NamingRule.Upper:
                    return fieldName.ToUpper();

                case NamingRule.Lower:
                    return fieldName.ToLower();

                case NamingRule.UnderLineLower:
                    return GetUnderlineLowerName(fieldName);

                default:
                    TkDebug.ThrowImpossibleCode(null);
                    return string.Empty;
            }
        }

        public static string GetUnderlineLowerName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return fieldName;

            StringBuilder builder = new StringBuilder();
            foreach (char c in fieldName)
            {
                if (char.IsUpper(c))
                {
                    if (builder.Length > 0)
                        builder.Append("_");
                    builder.Append(char.ToLower(c, ObjectUtil.SysCulture));
                }
                else
                    builder.Append(c);
            }

            return builder.ToString();
        }

        private static char NibbleToHex(byte nibble, bool lowerCase)
        {
            return ((nibble < 10) ? ((char)(nibble + 0x30)) :
                ((char)((nibble - 10) + (lowerCase ? 0x61 : 0x41))));
        }

        public static string BinaryToHex(byte[] data, bool lowerCase)
        {
            if (data == null)
                return null;
            char[] chArray = new char[data.Length << 1];
            for (int i = 0; i < data.Length; i++)
            {
                byte item = data[i];
                int index = i << 1;
                chArray[index] = NibbleToHex((byte)(item >> 4), lowerCase);
                chArray[index + 1] = NibbleToHex((byte)(item & 15), lowerCase);
            }
            return new string(chArray);
        }

        public static string TruncString(string value, int length)
        {
            if (value == null)
                return string.Empty;

            if (value.Length > (length >> 1))
            {
                byte[] bytes = GB_ENCODING.GetBytes(value);
                if (bytes.Length > length)
                {
                    string result = GB_ENCODING.GetString(bytes, 0, length);
                    return result.Substring(0, result.Length - 1);
                }
            }
            return value;
        }

        internal static Encoding GetSafeEncoding()
        {
            try
            {
                return Encoding.GetEncoding("gbk");
            }
            catch
            {
                return Encoding.Default;
            }
        }
    }
}