using System;
using System.Globalization;
using System.IO;

namespace YJC.Toolkit.Sys.Json
{
    internal class JsonTextReader : JsonReader
    {
        public static readonly string True = "true";
        public static readonly string False = "false";
        public static readonly string Null = "null";
        public static readonly string Undefined = "undefined";
        public static readonly string PositiveInfinity = "Infinity";
        public static readonly string NegativeInfinity = "-Infinity";
        public static readonly string NaN = "NaN";
        internal static long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        private readonly TextReader fReader;
        private readonly StringBuffer fBuffer;
        private char fCurrentChar;
        private int fCurrentLinePosition;
        private int fCurrentLineNumber;
        private bool fCurrentCharCarriageReturn;
        private bool fFirstRead;

        public JsonTextReader(TextReader reader)
        {
            TkDebug.AssertArgumentNull(reader, "reader", null);

            fReader = reader;
            fBuffer = new StringBuffer(4096);
            fCurrentLineNumber = 1;
            fFirstRead = false;
        }

        public int CurrentLinePosition
        {
            get
            {
                return fCurrentLinePosition;
            }
        }

        public int CurrentLineNumber
        {
            get
            {
                return fCurrentLineNumber;
            }
        }

        internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
        {
            DateTime dateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);

            return dateTime;
        }

        private void ParseString(char quote)
        {
            bool stringTerminated = false;
            bool hexNumber = false;
            int hexCount = 0;

            while (!stringTerminated && MoveNext())
            {
                if (hexNumber)
                    hexCount++;

                switch (fCurrentChar)
                {
                    case '\\':
                        if (MoveNext())
                        {
                            switch (fCurrentChar)
                            {
                                case 'b':
                                    fBuffer.Append('\b');
                                    break;
                                case 't':
                                    fBuffer.Append('\t');
                                    break;
                                case 'n':
                                    fBuffer.Append('\n');
                                    break;
                                case 'f':
                                    fBuffer.Append('\f');
                                    break;
                                case 'r':
                                    fBuffer.Append('\r');
                                    break;
                                case 'u':
                                    // note the start of a hex character
                                    hexNumber = true;
                                    break;
                                default:
                                    fBuffer.Append(fCurrentChar);
                                    break;
                            }
                        }
                        else
                        {
                            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                "非正常结束的字符串. 期望的分隔符: {0}. 在第{1}行, 第{2}列.",
                                quote, fCurrentLineNumber, fCurrentLinePosition), this);
                        }
                        break;
                    case '"':
                    case '\'':
                        if (fCurrentChar == quote)
                            stringTerminated = true;
                        else
                            goto default;
                        break;
                    default:
                        fBuffer.Append(fCurrentChar);
                        break;
                }

                if (hexCount == 4)
                {
                    // remove hex characters from buffer, convert to char and then add
                    string hexString = fBuffer.ToString(fBuffer.Position - 4, 4);
                    char hexChar = Convert.ToChar(int.Parse(hexString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));

                    fBuffer.Position = fBuffer.Position - 4;
                    fBuffer.Append(hexChar);

                    hexNumber = false;
                    hexCount = 0;
                }
            }

            TkDebug.Assert(stringTerminated, string.Format(ObjectUtil.SysCulture,
                "非正常结束的字符串. 期望的分隔符: {0}. 在第{1}行, 第{2}列.",
                quote, fCurrentLineNumber, fCurrentLinePosition), this);


            ClearCurrentChar();
            string text = fBuffer.ToString();
            fBuffer.Position = 0;

            if (text.StartsWith("/Date(", StringComparison.Ordinal) && text.EndsWith(")/", StringComparison.Ordinal))
            {
                ParseDate(text);
            }
            else
            {
                SetToken(JsonToken.String, text);
                QuoteChar = quote;
            }
        }

        protected override void SetToken(JsonToken newToken, object value)
        {
            base.SetToken(newToken, value);

            switch (newToken)
            {
                case JsonToken.StartObject:
                    ClearCurrentChar();
                    break;
                case JsonToken.StartArray:
                    ClearCurrentChar();
                    break;
                case JsonToken.StartConstructor:
                    ClearCurrentChar();
                    break;
                case JsonToken.EndObject:
                    ClearCurrentChar();
                    break;
                case JsonToken.EndArray:
                    ClearCurrentChar();
                    break;
                case JsonToken.EndConstructor:
                    ClearCurrentChar();
                    break;
                case JsonToken.PropertyName:
                    ClearCurrentChar();
                    break;
            }
        }

        private void ParseDate(string text)
        {
            string value = text.Substring(6, text.Length - 8);
            DateTimeKind kind = DateTimeKind.Utc;

            int index = value.IndexOf('+', 1);

            if (index == -1)
                index = value.IndexOf('-', 1);

            if (index != -1)
            {
                kind = DateTimeKind.Local;
                value = value.Substring(0, index);
            }

            long javaScriptTicks = long.Parse(value, ObjectUtil.SysCulture);
            DateTime utcDateTime = ConvertJavaScriptTicksToDateTime(javaScriptTicks);
            DateTime dateTime;

            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    break;
                case DateTimeKind.Local:
                    dateTime = utcDateTime.ToLocalTime();
                    break;
                default:
                    dateTime = utcDateTime;
                    break;
            }

            SetToken(JsonToken.Date, dateTime);
        }

        private const int LineFeedValue = '\n';
        private const int CarriageReturnValue = '\r';

        private bool MoveNext()
        {
            int value = fReader.Read();

            if (!fFirstRead && value == 65279) //UTF8的第一个字符 
                value = fReader.Read();
            fFirstRead = true;

            switch (value)
            {
                case -1:
                    return false;
                case CarriageReturnValue:
                    fCurrentLineNumber++;
                    fCurrentLinePosition = 0;
                    fCurrentCharCarriageReturn = true;
                    break;
                case LineFeedValue:
                    if (!fCurrentCharCarriageReturn)
                        fCurrentLineNumber++;

                    fCurrentLinePosition = 0;
                    fCurrentCharCarriageReturn = false;
                    break;
                default:
                    fCurrentLinePosition++;
                    fCurrentCharCarriageReturn = false;
                    break;
            }

            fCurrentChar = (char)value;
            return true;
        }

        private bool HasNext()
        {
            return (fReader.Peek() != -1);
        }

        private char PeekNext()
        {
            return (char)fReader.Peek();
        }

        private void ClearCurrentChar()
        {
            fCurrentChar = '\0';
        }

        public override bool Read()
        {
            while (true)
            {
                if (fCurrentChar == '\0')
                {
                    if (!MoveNext())
                        return false;
                }

                switch (CurrentState)
                {
                    case State.Start:
                    case State.Property:
                    case State.Array:
                    case State.ArrayStart:
                    case State.Constructor:
                    case State.ConstructorStart:
                        return ParseValue();
                    case State.Complete:
                        break;
                    case State.Object:
                    case State.ObjectStart:
                        return ParseObject();
                    case State.PostValue:
                        // returns true if it hits
                        // end of object or array
                        if (ParsePostValue())
                            return true;
                        break;
                    case State.Closed:
                        break;
                    case State.Error:
                        break;
                    default:
                        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            "不是期望的状态: {0}. 在第{1}行, 第{2}列.",
                            CurrentState, fCurrentLineNumber, fCurrentLinePosition), this);
                        break;
                }
            }
        }

        private bool ParsePostValue()
        {
            do
            {
                switch (fCurrentChar)
                {
                    case '}':
                        SetToken(JsonToken.EndObject);
                        ClearCurrentChar();
                        return true;
                    case ']':
                        SetToken(JsonToken.EndArray);
                        ClearCurrentChar();
                        return true;
                    case ')':
                        SetToken(JsonToken.EndConstructor);
                        ClearCurrentChar();
                        return true;
                    case '/':
                        ParseComment();
                        return true;
                    case ',':
                        SetStateBasedOnCurrent();
                        ClearCurrentChar();
                        return false;
                    default:
                        if (char.IsWhiteSpace(fCurrentChar))
                        {
                            // eat whitespace
                            ClearCurrentChar();
                        }
                        else
                        {
                            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                "在分析值时，碰到一个非期望的字符 : {0}. 在第{1}行, 第{2}列.",
                                fCurrentChar, fCurrentLineNumber, fCurrentLinePosition), this);
                        }
                        break;
                }
            } while (MoveNext());

            return false;
        }

        private bool ParseObject()
        {
            do
            {
                switch (fCurrentChar)
                {
                    case '}':
                        SetToken(JsonToken.EndObject);
                        return true;
                    case '/':
                        ParseComment();
                        return true;
                    case ',':
                        SetToken(JsonToken.Undefined);
                        return true;
                    default:
                        if (char.IsWhiteSpace(fCurrentChar))
                        {
                            // eat
                        }
                        else
                        {
                            return ParseProperty();
                        }
                        break;
                }
            } while (MoveNext());

            return false;
        }

        private bool ParseProperty()
        {
            char quoteChar = '\0';

            if (ValidIdentifierChar())
            {
                quoteChar = '\0';
                ParseUnquotedProperty();
            }
            else if (fCurrentChar == '"' || fCurrentChar == '\'')
            {
                quoteChar = fCurrentChar;
                ParseQuotedProperty(fCurrentChar);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "非法的属性标识符: {0}. 在第{1}行, 第{2}列.",
                    fCurrentChar, fCurrentLineNumber, fCurrentLinePosition), this);
            }

            if (fCurrentChar != ':')
            {
                MoveNext();

                // finished property. skip any whitespace and move to colon
                EatWhitespace(false);

                TkDebug.Assert(fCurrentChar == ':', string.Format(ObjectUtil.SysCulture,
                    "在分析属性名称后碰见非法字符, 期望':' 但实际是: {0}. 在第{1}行, 第{2}列.",
                    fCurrentChar, fCurrentLineNumber, fCurrentLinePosition), this);

            }

            SetToken(JsonToken.PropertyName, fBuffer.ToString());
            QuoteChar = quoteChar;
            fBuffer.Position = 0;

            return true;
        }

        private void ParseQuotedProperty(char quoteChar)
        {
            while (MoveNext())
            {
                if (fCurrentChar == quoteChar)
                {
                    return;
                }
                else
                {
                    fBuffer.Append(fCurrentChar);
                }
            }

            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "引号没有配对. 期望获得: {0}. 在第{1}行, 第{2}列.",
                quoteChar, fCurrentLineNumber, fCurrentLinePosition), this);
        }

        private bool ValidIdentifierChar()
        {
            return (char.IsLetterOrDigit(fCurrentChar) || fCurrentChar == '_' || fCurrentChar == '$');
        }

        private void ParseUnquotedProperty()
        {
            fBuffer.Append(fCurrentChar);

            while (MoveNext())
            {
                if (char.IsWhiteSpace(fCurrentChar) || fCurrentChar == ':')
                {
                    break;
                }
                else if (ValidIdentifierChar())
                {
                    fBuffer.Append(fCurrentChar);
                }
                else
                {
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "非法的js属性标识符: {0}. 在第{1}行, 第{2}列.",
                        fCurrentChar, fCurrentLineNumber, fCurrentLinePosition), this);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability",
            "CA1502:AvoidExcessiveComplexity")]
        private bool ParseValue()
        {
            do
            {
                switch (fCurrentChar)
                {
                    case '"':
                    case '\'':
                        ParseString(fCurrentChar);
                        return true;
                    case 't':
                        ParseTrue();
                        return true;
                    case 'f':
                        ParseFalse();
                        return true;
                    case 'n':
                        if (HasNext())
                        {
                            char next = PeekNext();

                            if (next == 'u')
                                ParseNull();
                            else if (next == 'e')
                                ParseConstructor();
                            else
                                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                    "在分析值时,遇见非法字符: {0}. 在第{1}行, 第{2}列.",
                                    fCurrentChar, fCurrentLineNumber, fCurrentLinePosition), this);
                        }
                        else
                        {
                            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                "不是期望的结束. 在第{0}行, 第{1}列.", fCurrentLineNumber, fCurrentLinePosition), this);

                        }
                        return true;
                    case 'N':
                        ParseNumberNaN();
                        return true;
                    case 'I':
                        ParseNumberPositiveInfinity();
                        return true;
                    case '-':
                        if (PeekNext() == 'I')
                            ParseNumberNegativeInfinity();
                        else
                            ParseNumber();
                        return true;
                    case '/':
                        ParseComment();
                        return true;
                    case 'u':
                        ParseUndefined();
                        return true;
                    case '{':
                        SetToken(JsonToken.StartObject);
                        return true;
                    case '[':
                        SetToken(JsonToken.StartArray);
                        return true;
                    case '}':
                        SetToken(JsonToken.EndObject);
                        return true;
                    case ']':
                        SetToken(JsonToken.EndArray);
                        return true;
                    case ',':
                        SetToken(JsonToken.Undefined);
                        return true;
                    case ')':
                        SetToken(JsonToken.EndConstructor);
                        return true;
                    default:
                        if (char.IsWhiteSpace(fCurrentChar))
                        {
                            // eat
                        }
                        else if (char.IsNumber(fCurrentChar) || fCurrentChar == '-' || fCurrentChar == '.')
                        {
                            ParseNumber();
                            return true;
                        }
                        else
                        {
                            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                "在分析值时,遇见非法字符: {0}. 在第{1}行, 第{2}列.",
                                fCurrentChar, fCurrentLineNumber, fCurrentLinePosition), this);
                        }
                        break;
                }
            } while (MoveNext());

            return false;
        }

        private bool EatWhitespace(bool oneOrMore)
        {
            bool whitespace = false;
            while (char.IsWhiteSpace(fCurrentChar))
            {
                whitespace = true;
                MoveNext();
            }

            return (!oneOrMore || whitespace);
        }

        private void ParseConstructor()
        {
            if (MatchValue("new", true))
            {
                if (EatWhitespace(true))
                {
                    while (char.IsLetter(fCurrentChar))
                    {
                        fBuffer.Append(fCurrentChar);
                        MoveNext();
                    }

                    EatWhitespace(false);

                    if (fCurrentChar != '(')
                        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            "在分析建构函数时,遇见非法字符: {0}. 在第{1}行, 第{2}列.",
                            fCurrentChar, fCurrentLineNumber, fCurrentLinePosition), this);

                    string constructorName = fBuffer.ToString();
                    fBuffer.Position = 0;

                    SetToken(JsonToken.StartConstructor, constructorName);
                }
            }
        }

        private void ParseNumber()
        {
            bool end = false;
            do
            {
                if (CurrentIsSeperator())
                    end = true;
                else
                    fBuffer.Append(fCurrentChar);

            } while (!end && MoveNext());

            // hit the end of the reader before the number ended. clear the last number value
            if (!end)
                ClearCurrentChar();

            string number = fBuffer.ToString();
            object numberValue;
            JsonToken numberType;

            if (number.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1
              && number.IndexOf("e", StringComparison.OrdinalIgnoreCase) == -1)
            {
                numberValue = Convert.ToInt64(fBuffer.ToString(), ObjectUtil.SysCulture);
                numberType = JsonToken.Integer;
            }
            else
            {
                numberValue = Convert.ToDouble(fBuffer.ToString(), ObjectUtil.SysCulture);
                numberType = JsonToken.Float;
            }

            fBuffer.Position = 0;

            SetToken(numberType, numberValue);
        }

        private void ParseComment()
        {
            MoveNext();

            if (fCurrentChar == '*')
            {
                while (MoveNext())
                {
                    if (fCurrentChar == '*')
                    {
                        if (MoveNext())
                        {
                            if (fCurrentChar == '/')
                            {
                                break;
                            }
                            else
                            {
                                fBuffer.Append('*');
                                fBuffer.Append(fCurrentChar);
                            }
                        }
                    }
                    else
                    {
                        fBuffer.Append(fCurrentChar);
                    }
                }
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "在分析注释时出错。期望: *. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }

            SetToken(JsonToken.Comment, fBuffer.ToString());

            fBuffer.Position = 0;

            ClearCurrentChar();
        }

        private bool MatchValue(string value)
        {
            int i = 0;
            do
            {
                if (fCurrentChar != value[i])
                {
                    break;
                }
                i++;
            }
            while (i < value.Length && MoveNext());

            return (i == value.Length);
        }

        private bool MatchValue(string value, bool noTrailingNonSeperatorCharacters)
        {
            // will match value and then move to the next character, checking that it is a seperator character
            bool match = MatchValue(value);

            if (!noTrailingNonSeperatorCharacters)
            {
                return match;
            }
            else
            {
                bool matchAndNoTrainingNonSeperatorCharacters = (match && (!MoveNext() || CurrentIsSeperator()));
                if (!CurrentIsSeperator())
                    ClearCurrentChar();

                return matchAndNoTrainingNonSeperatorCharacters;
            }
        }

        private bool CurrentIsSeperator()
        {
            switch (fCurrentChar)
            {
                case '}':
                case ']':
                case ',':
                    return true;
                case '/':
                    // check next character to see if start of a comment
                    return (HasNext() && PeekNext() == '*');
                case ')':
                    if (CurrentState == State.Constructor || CurrentState == State.ConstructorStart)
                        return true;
                    break;
                default:
                    if (char.IsWhiteSpace(fCurrentChar))
                        return true;
                    break;
            }

            return false;
        }

        private void ParseTrue()
        {
            if (MatchValue(True, true))
            {
                SetToken(JsonToken.Boolean, true);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "分析True值时出错. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }
        }

        private void ParseNull()
        {
            if (MatchValue(Null, true))
            {
                SetToken(JsonToken.Null);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "分析Null时出错. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }
        }

        private void ParseUndefined()
        {
            if (MatchValue(Undefined, true))
            {
                SetToken(JsonToken.Undefined);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "分析undefined值时出错. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }
        }

        private void ParseFalse()
        {
            if (MatchValue(False, true))
            {
                SetToken(JsonToken.Boolean, false);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "分析False值时出错. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }
        }

        private void ParseNumberNegativeInfinity()
        {
            if (MatchValue(NegativeInfinity, true))
            {
                SetToken(JsonToken.Float, double.NegativeInfinity);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "分析负无穷时出错. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }
        }

        private void ParseNumberPositiveInfinity()
        {
            if (MatchValue(PositiveInfinity, true))
            {
                SetToken(JsonToken.Float, double.PositiveInfinity);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "分析正无穷时出错. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }
        }

        private void ParseNumberNaN()
        {
            if (MatchValue(NaN, true))
            {
                SetToken(JsonToken.Float, double.NaN);
            }
            else
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "分析NaN值时出错. 在第{0}行, 第{1}列.",
                    fCurrentLineNumber, fCurrentLinePosition), this);
            }
        }

        public override void Close()
        {
            base.Close();

            if (fReader != null)
                fReader.Dispose();

            if (fBuffer != null)
                fBuffer.Clear();
        }
    }
}
