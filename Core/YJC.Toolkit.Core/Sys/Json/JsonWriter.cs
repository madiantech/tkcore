using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys.Json
{
    internal abstract class JsonWriter : IDisposable
    {
        private enum State
        {
            Start,
            Property,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            ConstructorStart,
            Constructor,
            Bytes,
            Closed,
            Error
        }

        // array that gives a new state based on the current state an the token being written
        private static readonly State[][] stateArray = new[] {
//                      Start                   PropertyName            ObjectStart         Object            ArrayStart              Array                   ConstructorStart        Constructor             Closed          Error
//                        
/* None             */new[]{ State.Error,            State.Error,            State.Error,        State.Error,      State.Error,            State.Error,            State.Error,            State.Error,            State.Error,    State.Error },
/* StartObject      */new[]{ State.ObjectStart,      State.ObjectStart,      State.Error,        State.Error,      State.ObjectStart,      State.ObjectStart,      State.ObjectStart,      State.ObjectStart,      State.Error,    State.Error },
/* StartArray       */new[]{ State.ArrayStart,       State.ArrayStart,       State.Error,        State.Error,      State.ArrayStart,       State.ArrayStart,       State.ArrayStart,       State.ArrayStart,       State.Error,    State.Error },
/* StartConstructor */new[]{ State.ConstructorStart, State.ConstructorStart, State.Error,        State.Error,      State.ConstructorStart, State.ConstructorStart, State.ConstructorStart, State.ConstructorStart, State.Error,    State.Error },
/* StartProperty    */new[]{ State.Property,         State.Error,            State.Property,     State.Property,   State.Error,            State.Error,            State.Error,            State.Error,            State.Error,    State.Error },
/* Comment          */new[]{ State.Start,            State.Property,         State.ObjectStart,  State.Object,     State.ArrayStart,       State.Array,            State.Constructor,      State.Constructor,      State.Error,    State.Error },
/* Raw              */new[]{ State.Start,            State.Property,         State.ObjectStart,  State.Object,     State.ArrayStart,       State.Array,            State.Constructor,      State.Constructor,      State.Error,    State.Error },
/* Value            */new[]{ State.Start,            State.Object,           State.Error,        State.Error,      State.Array,            State.Array,            State.Constructor,      State.Constructor,      State.Error,    State.Error },
		};

        private int fTop;
        private readonly List<JsonTokenType> fStack;
        private State fCurrentState;

        /// <summary>
        /// Creates an instance of the <c>JsonWriter</c> class. 
        /// </summary>
        public JsonWriter()
        {
            fStack = new List<JsonTokenType>(8);
            fStack.Add(JsonTokenType.None);
            fCurrentState = State.Start;
            Formatting = JsonFormatting.None;
        }

        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <value>The top.</value>
        protected internal int Top
        {
            get
            {
                return fTop;
            }
        }

        public JsonWriteState WriteState
        {
            get
            {
                switch (fCurrentState)
                {
                    case State.Error:
                        return JsonWriteState.Error;
                    case State.Closed:
                        return JsonWriteState.Closed;
                    case State.Object:
                    case State.ObjectStart:
                        return JsonWriteState.Object;
                    case State.Array:
                    case State.ArrayStart:
                        return JsonWriteState.Array;
                    case State.Constructor:
                    case State.ConstructorStart:
                        return JsonWriteState.Constructor;
                    case State.Property:
                        return JsonWriteState.Property;
                    case State.Start:
                        return JsonWriteState.Start;
                    default:
                        TkDebug.ThrowToolkitException("Invalid state: " + fCurrentState, this);
                        return JsonWriteState.Error;
                }
            }
        }

        public JsonFormatting Formatting { get; set; }

        private void Push(JsonTokenType value)
        {
            fTop++;
            if (fStack.Count <= fTop)
                fStack.Add(value);
            else
                fStack[fTop] = value;
        }

        private JsonTokenType Pop()
        {
            JsonTokenType value = Peek();
            fTop--;

            return value;
        }

        private JsonTokenType Peek()
        {
            return fStack[fTop];
        }

        public abstract void Flush();

        public virtual void Close()
        {
            AutoCompleteAll();
        }

        public virtual void WriteStartObject()
        {
            AutoComplete(JsonToken.StartObject);
            Push(JsonTokenType.Object);
        }

        public void WriteEndObject()
        {
            AutoCompleteClose(JsonToken.EndObject);
        }

        public virtual void WriteStartArray()
        {
            AutoComplete(JsonToken.StartArray);
            Push(JsonTokenType.Array);
        }

        public void WriteEndArray()
        {
            AutoCompleteClose(JsonToken.EndArray);
        }

        public virtual void WriteStartConstructor(string name)
        {
            AutoComplete(JsonToken.StartConstructor);
            Push(JsonTokenType.Constructor);
        }

        public void WriteEndConstructor()
        {
            AutoCompleteClose(JsonToken.EndConstructor);
        }

        public virtual void WritePropertyName(string name)
        {
            AutoComplete(JsonToken.PropertyName);
        }

        public void WriteEnd()
        {
            WriteEnd(Peek());
        }

        public void WriteToken(JsonReader reader)
        {
            TkDebug.AssertArgumentNull(reader, "reader", this);

            int initialDepth;

            if (reader.TokenType == JsonToken.None)
                initialDepth = -1;
            else if (!IsStartToken(reader.TokenType))
                initialDepth = reader.Depth + 1;
            else
                initialDepth = reader.Depth;

            WriteToken(reader, initialDepth);
        }

        internal void WriteToken(JsonReader reader, int initialDepth)
        {
            do
            {
                switch (reader.TokenType)
                {
                    case JsonToken.None:
                        // read to next
                        break;
                    case JsonToken.StartObject:
                        WriteStartObject();
                        break;
                    case JsonToken.StartArray:
                        WriteStartArray();
                        break;
                    case JsonToken.StartConstructor:
                        string constructorName = reader.Value.ToString();
                        // write a JValue date when the constructor is for a date
                        if (string.Compare(constructorName, "Date", StringComparison.Ordinal) == 0)
                            WriteConstructorDate(reader);
                        else
                            WriteStartConstructor(reader.Value.ToString());
                        break;
                    case JsonToken.PropertyName:
                        WritePropertyName(reader.Value.ToString());
                        break;
                    case JsonToken.Comment:
                        WriteComment(reader.Value.ToString());
                        break;
                    case JsonToken.Integer:
                        WriteValue((long)reader.Value);
                        break;
                    case JsonToken.Float:
                        WriteValue((double)reader.Value);
                        break;
                    case JsonToken.String:
                        WriteValue(reader.Value.ToString());
                        break;
                    case JsonToken.Boolean:
                        WriteValue((bool)reader.Value);
                        break;
                    case JsonToken.Null:
                        WriteNull();
                        break;
                    case JsonToken.Undefined:
                        WriteUndefined();
                        break;
                    case JsonToken.EndObject:
                        WriteEndObject();
                        break;
                    case JsonToken.EndArray:
                        WriteEndArray();
                        break;
                    case JsonToken.EndConstructor:
                        WriteEndConstructor();
                        break;
                    case JsonToken.Date:
                        WriteValue((DateTime)reader.Value);
                        break;
                    case JsonToken.Raw:
                        WriteRawValue((string)reader.Value);
                        break;
                    case JsonToken.Bytes:
                        WriteValue((byte[])reader.Value);
                        break;
                    default:
                        TkDebug.ThrowIfNoGlobalVariable();
                        break;
                }
            }
            while (
              initialDepth - 1 < reader.Depth - (IsEndToken(reader.TokenType) ? 1 : 0)
              && reader.Read());
        }

        private void WriteConstructorDate(JsonReader reader)
        {
            if (!reader.Read())
                TkDebug.ThrowToolkitException("在读日期构造函数时，出现非期望的结束", this);
            if (reader.TokenType != JsonToken.Integer)
                TkDebug.ThrowToolkitException("在读日期构造函数时，出现非期望的token。期望整数, 实际是"
                    + reader.TokenType, this);

            long ticks = (long)reader.Value;
            DateTime date = JsonConvert.ConvertJavaScriptTicksToDateTime(ticks);

            if (!reader.Read())
                TkDebug.ThrowToolkitException("在读日期构造函数时，出现非期望的结束", this);
            if (reader.TokenType != JsonToken.EndConstructor)
                TkDebug.ThrowToolkitException("在读日期构造函数时，出现非期望的token。期望结束构造函数, 实际是"
                    + reader.TokenType, this);

            WriteValue(date);
        }

        private static bool IsEndToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                case JsonToken.EndArray:
                case JsonToken.EndConstructor:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsStartToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.StartObject:
                case JsonToken.StartArray:
                case JsonToken.StartConstructor:
                    return true;
                default:
                    return false;
            }
        }

        private void WriteEnd(JsonTokenType type)
        {
            switch (type)
            {
                case JsonTokenType.Object:
                    WriteEndObject();
                    break;
                case JsonTokenType.Array:
                    WriteEndArray();
                    break;
                case JsonTokenType.Constructor:
                    WriteEndConstructor();
                    break;
                default:
                    TkDebug.ThrowToolkitException("在写结束是，非期望的类型：" + type, this);
                    break;
            }
        }

        private void AutoCompleteAll()
        {
            while (fTop > 0)
            {
                WriteEnd();
            }
        }

        private JsonTokenType GetTypeForCloseToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    return JsonTokenType.Object;
                case JsonToken.EndArray:
                    return JsonTokenType.Array;
                case JsonToken.EndConstructor:
                    return JsonTokenType.Constructor;
                default:
                    TkDebug.ThrowToolkitException("非期望的类型: " + token, this);
                    return JsonTokenType.None;
            }
        }

        private JsonToken GetCloseTokenForType(JsonTokenType type)
        {
            switch (type)
            {
                case JsonTokenType.Object:
                    return JsonToken.EndObject;
                case JsonTokenType.Array:
                    return JsonToken.EndArray;
                case JsonTokenType.Constructor:
                    return JsonToken.EndConstructor;
                default:
                    TkDebug.ThrowToolkitException("非期望的结束类型: " + type, this);
                    return JsonToken.None;
            }
        }

        private void AutoCompleteClose(JsonToken tokenBeingClosed)
        {
            // write closing symbol and calculate new state

            int levelsToComplete = 0;

            for (int i = 0; i < fTop; i++)
            {
                int currentLevel = fTop - i;

                if (fStack[currentLevel] == GetTypeForCloseToken(tokenBeingClosed))
                {
                    levelsToComplete = i + 1;
                    break;
                }
            }

            TkDebug.Assert(levelsToComplete != 0, "No token to close.", this);
            //if (levelsToComplete == 0)
            //    throw new JsonWriterException("");

            for (int i = 0; i < levelsToComplete; i++)
            {
                JsonToken token = GetCloseTokenForType(Pop());

                if (fCurrentState != State.ObjectStart && fCurrentState != State.ArrayStart)
                    WriteIndent();

                WriteEnd(token);
            }

            JsonTokenType currentLevelType = Peek();

            switch (currentLevelType)
            {
                case JsonTokenType.Object:
                    fCurrentState = State.Object;
                    break;
                case JsonTokenType.Array:
                    fCurrentState = State.Array;
                    break;
                case JsonTokenType.Constructor:
                    fCurrentState = State.Array;
                    break;
                case JsonTokenType.None:
                    fCurrentState = State.Start;
                    break;
                default:
                    TkDebug.ThrowToolkitException("非期望的JSON类型: " + currentLevelType, this);
                    break;
            }
        }

        /// <summary>
        /// Writes the specified end token.
        /// </summary>
        /// <param name="token">The end token to write.</param>
        protected virtual void WriteEnd(JsonToken token)
        {
        }

        /// <summary>
        /// Writes indent characters.
        /// </summary>
        protected virtual void WriteIndent()
        {
        }

        /// <summary>
        /// Writes the JSON value delimiter.
        /// </summary>
        protected virtual void WriteValueDelimiter()
        {
        }

        /// <summary>
        /// Writes an indent space.
        /// </summary>
        protected virtual void WriteIndentSpace()
        {
        }

        internal void AutoComplete(JsonToken tokenBeingWritten)
        {
            int token;

            switch (tokenBeingWritten)
            {
                default:
                    token = (int)tokenBeingWritten;
                    break;
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    // a value is being written
                    token = 7;
                    break;
            }

            // gets new state based on the current state and what is being written
            State newState = stateArray[token][(int)fCurrentState];

            if (newState == State.Error)
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "Token {0} in state {1} would result in an invalid JavaScript object.",
                    tokenBeingWritten, fCurrentState), this);

            if ((fCurrentState == State.Object || fCurrentState == State.Array ||
                fCurrentState == State.Constructor) && tokenBeingWritten != JsonToken.Comment)
            {
                WriteValueDelimiter();
            }
            else if (fCurrentState == State.Property)
            {
                if (Formatting == JsonFormatting.Indented)
                    WriteIndentSpace();
            }

            JsonWriteState writeState = WriteState;

            // don't indent a property when it is the first token to be written (i.e. at the start)
            if ((tokenBeingWritten == JsonToken.PropertyName && writeState != JsonWriteState.Start) ||
              writeState == JsonWriteState.Array || writeState == JsonWriteState.Constructor)
            {
                WriteIndent();
            }

            fCurrentState = newState;
        }

        #region WriteValue methods
        public virtual void WriteNull()
        {
            AutoComplete(JsonToken.Null);
        }

        public virtual void WriteUndefined()
        {
            AutoComplete(JsonToken.Undefined);
        }

        public virtual void WriteRaw(string json)
        {
        }

        public virtual void WriteRawValue(string json)
        {
            AutoComplete(JsonToken.Undefined);
            WriteRaw(json);
        }

        public virtual void WriteValue(string value)
        {
            AutoComplete(JsonToken.String);
        }

        public virtual void WriteValue(int value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(uint value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(long value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(ulong value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(float value)
        {
            AutoComplete(JsonToken.Float);
        }

        public virtual void WriteValue(double value)
        {
            AutoComplete(JsonToken.Float);
        }

        public virtual void WriteValue(bool value)
        {
            AutoComplete(JsonToken.Boolean);
        }

        public virtual void WriteValue(short value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(ushort value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(char value)
        {
            AutoComplete(JsonToken.String);
        }

        public virtual void WriteValue(byte value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(sbyte value)
        {
            AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(decimal value)
        {
            AutoComplete(JsonToken.Float);
        }

        public virtual void WriteValue(DateTime value)
        {
            AutoComplete(JsonToken.Date);
        }

        public virtual void WriteValue(DateTimeOffset value)
        {
            AutoComplete(JsonToken.Date);
        }

        public virtual void WriteValue(int? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(uint? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(long? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(ulong? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(float? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(double? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(bool? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(short? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(ushort? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(char? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(byte? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(sbyte? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(decimal? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(DateTime? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(DateTimeOffset? value)
        {
            if (value == null)
                WriteNull();
            else
                WriteValue(value.Value);
        }

        public virtual void WriteValue(byte[] value)
        {
            if (value == null)
                WriteNull();
            else
                AutoComplete(JsonToken.Bytes);
        }

        public virtual void WriteValue(object value)
        {
            if (value == null)
            {
                WriteNull();
                return;
            }
            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.String:
                        WriteValue(convertible.ToString(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Char:
                        WriteValue(convertible.ToChar(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Boolean:
                        WriteValue(convertible.ToBoolean(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.SByte:
                        WriteValue(convertible.ToSByte(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Int16:
                        WriteValue(convertible.ToInt16(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.UInt16:
                        WriteValue(convertible.ToUInt16(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Int32:
                        WriteValue(convertible.ToInt32(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Byte:
                        WriteValue(convertible.ToByte(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.UInt32:
                        WriteValue(convertible.ToUInt32(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Int64:
                        WriteValue(convertible.ToInt64(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.UInt64:
                        WriteValue(convertible.ToUInt64(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Single:
                        WriteValue(convertible.ToSingle(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Double:
                        WriteValue(convertible.ToDouble(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.DateTime:
                        WriteValue(convertible.ToDateTime(ObjectUtil.SysCulture));
                        return;
                    case TypeCode.Decimal:
                        WriteValue(convertible.ToDecimal(ObjectUtil.SysCulture));
                        return;
                    //case TypeCode.DBNull:
                    //    WriteNull();
                    //return;
                }
                return;
            }
            if (value is DateTimeOffset)
            {
                WriteValue((DateTimeOffset)value);
                return;
            }
            byte[] byteValue = value as byte[];
            if (byteValue != null)
            {
                WriteValue(byteValue);
                return;
            }

            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "不支持的类型: {0}。", value.GetType()), this);
        }
        #endregion

        public virtual void WriteComment(string text)
        {
            AutoComplete(JsonToken.Comment);
        }

        public virtual void WriteWhitespace(string ws)
        {
            if (ws != null)
                TkDebug.Assert(StringUtil.IsWhiteSpace(ws), "只有空白字符才能使用", this);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (WriteState != JsonWriteState.Closed)
                    Close();
            }
        }
    }
}