using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <summary>
    /// AbstractExpression ��ժҪ˵����
    /// </summary>
    internal sealed class ExpressionParser
    {
        private readonly List<string> fParamArray;

        private ExpressionParser()
        {
            fParamArray = new List<string>();
        }

        public string FormatString { get; private set; }

        public int ParamCount
        {
            get
            {
                return fParamArray.Count;
            }
        }

        public IEnumerable<string> ParamArray
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
                if (c == '{')
                {
                    result.Append(format, start, ptr - start - 1);

                    // check for escaped open bracket

                    if (format[ptr] == '{')
                    {
                        start = ptr++;
                        result.Append("{");
                        continue;
                    }

                    // parse specifier
                    int marcoStart = ptr;

                    while (format[ptr++] != '}')
                        if (ptr >= length)
                            break;

                    TkDebug.Assert(ptr <= length, string.Format(ObjectUtil.SysCulture,
                        "ɨ���ַ���{0}���֣�����ֻ��{{��û����֮ƥ���}}�������ַ���", format), format);

                    string marco = format.Substring(marcoStart, ptr - marcoStart - 1);
                    TkDebug.Assert(!string.IsNullOrEmpty(marco), string.Format(ObjectUtil.SysCulture,
                        "�ַ���{0}�д���{{��}}֮��û���κκ����ƣ��պ��ǲ��������", format), format);
                    fParamArray.Add(marco);

                    result.Append("{").Append(marcoCount++).Append("}");

                    start = ptr;
                }
                else if (c == '}' && ptr < length && format[ptr] == '}')
                {
                    result.Append(format, start, ptr - start - 1);
                    start = ptr++;
                    result.Append("}");
                }
                else if (c == '}')
                {
                    TkDebug.Assert(false, string.Format(ObjectUtil.SysCulture,
                        "ɨ���ַ���{0}���֣����ڵ�����}}�����Ҫ��ʾ}}����д����}}}}����������{{���", format), format);
                }
            }

            if (start < length)
                result.Append(format.Substring(start));

            FormatString = result.ToString();
        }

        public static ExpressionParser ParseExpression(string expression)
        {
            ExpressionParser parser = new ExpressionParser();
            parser.ParseExpr(expression);
            return parser;
        }
    }
}
