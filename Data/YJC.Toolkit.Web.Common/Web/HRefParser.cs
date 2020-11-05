using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    /// <summary>
    /// AbstractExpression 的摘要说明。
    /// </summary>
    public sealed class HRefParser
    {
        private readonly List<string> fParamArray;

        private HRefParser()
        {
            fParamArray = new List<string>();
        }

        public string FormatString { get; private set; }

        public List<string> ParamArray
        {
            get
            {
                return fParamArray;
            }
        }

        public void ParseExpr(string format)
        {
            StringBuilder result = new StringBuilder();

            int ptr = 0;
            int start = ptr;
            int length = format.Length;
            int marcoCount = 0;

            while (ptr < length)
            {
                char c = format[ptr++];
                if (c == '*')
                {
                    result.Append(format, start, ptr - start - 1);

                    // check for escaped open bracket

                    //if (format[ptr] == '{')
                    //{
                    //    start = ptr++;
                    //    result.Append("{");
                    //    continue;
                    //}

                    // parse specifier
                    int marcoStart = ptr;

                    while (format[ptr++] != '*')
                        if (ptr >= length)
                            break;

                    TkDebug.Assert(ptr <= length, string.Format(ObjectUtil.SysCulture,
                        "扫描字符串{0}发现，存在只有*而没有与之匹配的*结束的字符串", format), format);

                    string marco = format.Substring(marcoStart, ptr - marcoStart - 1);
                    TkDebug.Assert(!string.IsNullOrEmpty(marco), string.Format(ObjectUtil.SysCulture,
                        "字符串{0}中存在*与*之间没有任何宏名称，空宏是不被允许的", format), format);
                    fParamArray.Add(marco);

                    result.Append("{").Append(marcoCount++).Append("}");

                    start = ptr;
                }
            }
            if (start < length)
                result.Append(format.Substring(start));

            FormatString = result.ToString();
        }

        public static HRefParser ParseExpression(string expression)
        {
            HRefParser parser = new HRefParser();
            parser.ParseExpr(expression);
            return parser;
        }
    }
}