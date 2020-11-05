using System.Text;
using System.Text.RegularExpressions;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class PinYinUtil
    {
        public readonly static Encoding GBEncoding = StringUtil.GetSafeEncoding();

        private static readonly Regex CHINESE_REGEX = new Regex(@"[\u4E00-\u9FA5]+");
        private const int BUFFER_SIZE = 1024;
        private static PinYinSql fPySql;

        internal static PinYinSql PySql
        {
            get
            {
                if (fPySql == null)
                {
                    fPySql = new PinYinSql();
                    fPySql.Load();
                }
                return fPySql;
            }
        }

        private static bool GetPinyin(char ch, out string pinyin)
        {
            short hash = GetHashIndex(ch);

            short[] hashArray = PyHash.hashes[hash];
            for (var i = 0; i < hashArray.Length; ++i)
            {
                short index = hashArray[i];
                string pyCode = PyCode.codes[index];
                var pos = pyCode.IndexOf(ch, 7);
                if (pos != -1)
                {
                    pinyin = pyCode.Substring(0, 6).Trim();
                    return true;
                }
            }
            pinyin = string.Empty;
            return false;
        }

        /// <summary>
        /// 取中文文本的拼音首字母
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <returns>返回中文对应的拼音首字母</returns>
        public static string GetPyHeader(string text)
        {
            return GetPyHeader(text, "#");
        }

        public static string GetPyHeader(string text, string unchineseMark)
        {
            text = text.Trim();
            StringBuilder chars = new StringBuilder();
            for (var i = 0; i < text.Length; ++i)
            {
                string py;
                char ch = text[i];
                int charIndex = (int)ch;
                if ((charIndex >= 65 && charIndex <= 90) || (charIndex >= 97 && charIndex <= 122))
                {
                    chars.Append(ch);
                }
                else if (CHINESE_REGEX.IsMatch(ch.ToString()))
                {
                    if (GetPinyin(ch, out py))
                    {
                        if (!string.IsNullOrEmpty(py))
                            chars.Append(py[0]);
                    }
                    else
                        chars.Append(unchineseMark);
                }
                else
                    chars.Append(unchineseMark);
            }

            return chars.ToString().ToUpper(ObjectUtil.SysCulture);
        }

        /// <summary>
        /// 取中文文本的拼音
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <returns>返回中文文本的拼音</returns>
        public static string GetPinyin(string text)
        {
            StringBuilder sbPinyin = new StringBuilder();
            for (var i = 0; i < text.Length; ++i)
            {
                string py;
                if (GetPinyin(text[i], out py))
                    sbPinyin.Append(py).Append(" ");
            }

            return sbPinyin.ToString().Trim();
        }

        /// <summary>
        /// 取文本索引值
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>文本索引值</returns>
        private static short GetHashIndex(char ch)
        {
            return (short)((uint)ch % PyCode.codes.Length);
        }

        private static string[] fStartChars = {"啊", "芭", "擦","搭","蛾","发","噶","哈","击","击","喀","垃","妈","拿","哦",
                                                 "啪","期","然", "撒","塌","挖","挖","挖","昔","压","匝"};

        private static string[] fEndChars = {"澳", "怖", "错","堕","贰","咐","过","祸","啊","骏","阔","络","穆","诺","沤",
                                               "瀑", "群","弱", "所","唾","啊","啊","误","迅","孕","座"};

        /// <summary>
        /// 将指定字段值的每个字符分割，这样可以生成同音查询的SQL
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns>生成的可以进行同音查询的SQL</returns>
        public static string GetCharFullCondition(TkDbContext context, string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
                return string.Empty;
            TkDebug.AssertArgumentNull(context, "context", null);
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", null);

            StringBuilder sql = new StringBuilder(BUFFER_SIZE);
            int i = 1;
            foreach (char c in fieldValue)
            {
                if (i > 1)
                    sql.Append(" AND ");
                char upChar = char.ToUpper(c, ObjectUtil.SysCulture);
                int index = (int)(upChar) - (int)'A';
                string startWord, endWord;
                if (index >= 0 && index < 26)
                {
                    startWord = fStartChars[index];
                    endWord = fEndChars[index];
                }
                else
                {
                    startWord = fStartChars[0];
                    endWord = fEndChars[0];
                }
                string subStr = context.ContextConfig.GetFunction("SUBSTRING", fieldName, i, 1);
                string extension = PySql[upChar];
                string exSQL = string.Empty;
                if (!string.IsNullOrEmpty(extension))
                    exSQL = string.Format(ObjectUtil.SysCulture, " OR {0} IN ({1})", subStr, extension);
                sql.AppendFormat(ObjectUtil.SysCulture, "({0} BETWEEN '{1}' AND '{2}'{3})", subStr, startWord, endWord, exSQL);
                ++i;
            }

            return sql.ToString();
        }

        private static int[] fStartBytes = {45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062,
                                              49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698,
                                              52698, 52980, 53689, 54481};

        private static int[] fEndBytes = {45252, 45760, 46317, 46825, 47009, 47296, 47613, 48118, 45217, 49061, 49323,
                                            49895, 50370, 50613, 50621, 50905, 51386, 51445, 52217, 52697, 45217, 45217,
                                            52979, 53688, 54480, 55289};

        /// <summary>
        /// 获得汉字的拼音
        /// </summary>
        /// <param name="hz">汉字</param>
        /// <returns>汉字拼音</returns>
        private static char GetCharPY(char hz)
        {
            byte[] arr = Encoding.Default.GetBytes(hz.ToString());
            if (arr.Length > 1)
            {
                int temp = arr[0];
                temp <<= 8; // * 256
                temp += arr[1];
                for (int i = 0; i < fStartBytes.Length; ++i)
                {
                    if (temp < fStartBytes[i])
                        break;
                    if (temp >= fStartBytes[i] && temp <= fEndBytes[i])
                        return (char)(i + 'a');
                }
            }
            return '\0';
        }

        /// <summary>
        /// 获得字符串的拼音
        /// </summary>
        /// <param name="hz">汉字字符串</param>
        /// <returns>字符串的拼音</returns>
        public static string GetPY(string hz)
        {
            if (string.IsNullOrEmpty(hz))
                return string.Empty;
            int hzLength = GBEncoding.GetByteCount(hz);
            if (hzLength == hz.Length)
                return hz;

            StringBuilder result = new StringBuilder();
            foreach (char c in hz)
            {
                char py = GetCharPY(c);
                if (py != '\0')
                    result.Append(py);
            }
            return result.ToString();
        }
    }
}