using System.IO;

namespace YJC.Toolkit.Sys.Json
{
    internal static class JavaScriptUtil
    {
        private static string GetEscapedValue(char delimiter, char c)
        {
            string escapedValue;
            switch (c)
            {
                case '\t':
                    escapedValue = @"\t";
                    break;
                case '\n':
                    escapedValue = @"\n";
                    break;
                case '\r':
                    escapedValue = @"\r";
                    break;
                case '\f':
                    escapedValue = @"\f";
                    break;
                case '\b':
                    escapedValue = @"\b";
                    break;
                case '\\':
                    escapedValue = @"\\";
                    break;
                case '\u0085':
                    escapedValue = @"\u0085";
                    break;
                case '\u2028':
                    escapedValue = @"\u2028";
                    break;
                case '\u2029':
                    escapedValue = @"\u2029";
                    break;
                case '\'':
                    escapedValue = (delimiter == '\'') ? @"\'" : null;
                    break;
                case '"':
                    escapedValue = (delimiter == '"') ? "\\\"" : null;
                    break;
                default:
                    escapedValue = (c <= '\u001f') ? c.AsUnicode() : null;
                    break;
            }
            return escapedValue;
        }

        public static void WriteEscapedJavaScriptString(TextWriter writer, string value,
            char delimiter, bool appendDelimiters)
        {
            // leading delimiter
            if (appendDelimiters)
                writer.Write(delimiter);

            if (value != null)
            {
                int lastWritePosition = 0;
                int skipped = 0;
                char[] chars = null;

                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    string escapedValue;

                    escapedValue = GetEscapedValue(delimiter, c);

                    if (escapedValue != null)
                    {
                        if (chars == null)
                            chars = value.ToCharArray();

                        // write skipped text
                        if (skipped > 0)
                        {
                            writer.Write(chars, lastWritePosition, skipped);
                            skipped = 0;
                        }

                        // write escaped value and note position
                        writer.Write(escapedValue);
                        lastWritePosition = i + 1;
                    }
                    else
                    {
                        skipped++;
                    }
                }

                // write any remaining skipped text
                if (skipped > 0)
                {
                    if (lastWritePosition == 0)
                        writer.Write(value);
                    else
                        writer.Write(chars, lastWritePosition, skipped);
                }
            }

            // trailing delimiter
            if (appendDelimiters)
                writer.Write(delimiter);
        }

        private static int? GetLength(string value)
        {
            if (value == null)
                return null;
            else
                return value.Length;
        }

        public static string ToEscapedJavaScriptString(string value,
            char delimiter, bool appendDelimiters)
        {
            using (StringWriter w = StringUtil.CreateStringWriter(GetLength(value) ?? 16))
            {
                WriteEscapedJavaScriptString(w, value, delimiter, appendDelimiters);
                return w.ToString();
            }
        }
    }
}