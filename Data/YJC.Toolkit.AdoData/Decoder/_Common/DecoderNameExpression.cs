using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    class DecoderNameExpression
    {
        private ExpressionParser fParser;

        public DecoderNameExpression(string expression)
        {
            fParser = ExpressionParser.ParseExpression(expression);
        }

        public string Execute(DataRow row)
        {
            //var values = (from expr in fParser.ParamArray
            //              select row[expr]).ToArray();
            object[] values = new object[fParser.ParamCount];
            int index = 0;
            foreach (string param in fParser.ParamArray)
            {
                if (param.Contains('`'))
                {
                    string[] arr = param.Split('`');
                    object value = row[arr[0]];
                    if (string.IsNullOrEmpty(value.ToString()))
                        values[index] = string.Empty;
                    else
                    {
                        string fmtStr = arr[1].Replace('[', '{').Replace(']', '}');
                        values[index] = string.Format(ObjectUtil.SysCulture, fmtStr, value);
                    }

                }
                else
                    values[index] = row[param];
                ++index;
            }
            return string.Format(ObjectUtil.SysCulture, fParser.FormatString, values);
        }
    }
}
