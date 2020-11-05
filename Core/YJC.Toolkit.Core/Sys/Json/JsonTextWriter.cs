using System;
using System.IO;

namespace YJC.Toolkit.Sys.Json
{
    internal class JsonTextWriter : JsonWriter
    {
        private readonly TextWriter fWriter;
        private Base64Encoder fBase64Encoder;
        private int fIndentation;
        private char fQuoteChar;

        private Base64Encoder Base64Encoder
        {
            get
            {
                if (fBase64Encoder == null)
                    fBase64Encoder = new Base64Encoder(fWriter);

                return fBase64Encoder;
            }
        }

        public int Indentation
        {
            get
            {
                return fIndentation;
            }
            set
            {
                TkDebug.AssertArgument(value >= 0, "value", "Indentation value must be greater than 0.", this);

                fIndentation = value;
            }
        }

        public char QuoteChar
        {
            get
            {
                return fQuoteChar;
            }
            set
            {
                TkDebug.AssertArgument(value == '"' || value == '\'', "value",
                    @"Invalid JavaScript string quote character. Valid quote characters are ' and "".", this);

                fQuoteChar = value;
            }
        }

        public char IndentChar { get; set; }

        public bool QuoteName { get; set; }

        public JsonTextWriter(TextWriter textWriter)
        {
            TkDebug.AssertArgumentNull(textWriter, "textWriter", null);

            fWriter = textWriter;
            fQuoteChar = '"';
            QuoteName = true;
            IndentChar = ' ';
            fIndentation = 2;
        }

        public override void Flush()
        {
            fWriter.Flush();
        }

        public override void Close()
        {
            base.Close();

            fWriter.Dispose();
        }

        public override void WriteStartObject()
        {
            base.WriteStartObject();

            fWriter.Write("{");
        }

        public override void WriteStartArray()
        {
            base.WriteStartArray();

            fWriter.Write("[");
        }

        public override void WriteStartConstructor(string name)
        {
            base.WriteStartConstructor(name);

            fWriter.Write("new ");
            fWriter.Write(name);
            fWriter.Write("(");
        }

        protected override void WriteEnd(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    fWriter.Write("}");
                    break;
                case JsonToken.EndArray:
                    fWriter.Write("]");
                    break;
                case JsonToken.EndConstructor:
                    fWriter.Write(")");
                    break;
                default:
                    TkDebug.ThrowToolkitException("Invalid JsonToken: " + token, this);
                    break;
            }
        }

        public override void WritePropertyName(string name)
        {
            base.WritePropertyName(name);

            JavaScriptUtil.WriteEscapedJavaScriptString(fWriter, name, fQuoteChar, QuoteName);

            fWriter.Write(':');
        }

        protected override void WriteIndent()
        {
            if (Formatting == JsonFormatting.Indented)
            {
                fWriter.Write(Environment.NewLine);

                // levels of indentation multiplied by the indent count
                int currentIndentCount = Top * fIndentation;

                for (int i = 0; i < currentIndentCount; i++)
                {
                    fWriter.Write(IndentChar);
                }
            }
        }

        protected override void WriteValueDelimiter()
        {
            fWriter.Write(',');
        }

        protected override void WriteIndentSpace()
        {
            fWriter.Write(' ');
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA1801:ReviewUnusedParameters", MessageId = "token")]
        private void WriteValueInternal(string value, JsonToken token)
        {
            fWriter.Write(value);
        }

        #region WriteValue methods
        public override void WriteNull()
        {
            base.WriteNull();
            WriteValueInternal(JsonConvert.Null, JsonToken.Null);
        }

        public override void WriteUndefined()
        {
            base.WriteUndefined();
            WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
        }

        public override void WriteRaw(string json)
        {
            base.WriteRaw(json);

            fWriter.Write(json);
        }

        public override void WriteValue(string value)
        {
            base.WriteValue(value);
            if (value == null)
                WriteValueInternal(JsonConvert.Null, JsonToken.Null);
            else
                JavaScriptUtil.WriteEscapedJavaScriptString(fWriter, value, fQuoteChar, true);
        }

        public override void WriteValue(int value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(uint value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(long value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(ulong value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(float value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
        }

        public override void WriteValue(double value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
        }

        public override void WriteValue(bool value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
        }

        public override void WriteValue(short value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(ushort value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(char value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(byte value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(sbyte value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
        }

        public override void WriteValue(decimal value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
        }

        public override void WriteValue(DateTime value)
        {
            base.WriteValue(value);
            JsonConvert.WriteDateTimeString(fWriter, value);
        }

        public override void WriteValue(byte[] value)
        {
            base.WriteValue(value);

            if (value != null)
            {
                fWriter.Write(fQuoteChar);
                Base64Encoder.Encode(value, 0, value.Length);
                Base64Encoder.Flush();
                fWriter.Write(fQuoteChar);
            }
        }

        public override void WriteValue(DateTimeOffset value)
        {
            base.WriteValue(value);
            WriteValueInternal(JsonConvert.ToString(value), JsonToken.Date);
        }
        #endregion

        public override void WriteComment(string text)
        {
            base.WriteComment(text);

            fWriter.Write("/*");
            fWriter.Write(text);
            fWriter.Write("*/");
        }

        public override void WriteWhitespace(string ws)
        {
            base.WriteWhitespace(ws);

            fWriter.Write(ws);
        }
    }
}