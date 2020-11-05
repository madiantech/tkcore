using System.IO;
using System.Text;
using System.Xml;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Sys.Json;

namespace YJC.Toolkit.Xml
{
    public sealed class XmlJsonWriter : XmlWriter
    {
        private readonly JsonTextWriter fJsonWriter;
        private int fLevel;
        private string fOldTableName;
        private WriteState fState;
        private ActionResultData fActionResult;

        public XmlJsonWriter(TextWriter writer)
        {
            fJsonWriter = new JsonTextWriter(writer);
            fState = WriteState.Start;
        }

        public XmlJsonWriter(TextWriter writer, ActionResultData result)
            : this(writer)
        {
            fActionResult = result;
        }

        public XmlJsonWriter(StringBuilder builder)
            : this(new StringWriter(builder, ObjectUtil.SysCulture))
        {
        }

        public XmlJsonWriter(StringBuilder builder, ActionResultData result)
            : this(new StringWriter(builder, ObjectUtil.SysCulture), result)
        {
        }

        public XmlJsonWriter(Stream stream, Encoding encoding)
            : this(new StreamWriter(stream, encoding))
        {
        }

        public XmlJsonWriter(Stream stream, Encoding encoding, ActionResultData result)
            : this(new StreamWriter(stream, encoding), result)
        {
        }

        public XmlJsonWriter(Stream stream)
            : this(new StreamWriter(stream))
        {
        }

        public XmlJsonWriter(Stream stream, ActionResultData result)
            : this(new StreamWriter(stream), result)
        {
        }

        private void WriteActionResult()
        {
            fJsonWriter.WritePropertyName("Result");
            fJsonWriter.WriteStartObject();
            fJsonWriter.WritePropertyName("Result");
            fJsonWriter.WriteValue(fActionResult.Result.ToString());
            fJsonWriter.WritePropertyName("Message");
            fJsonWriter.WriteValue(fActionResult.Message);
            fJsonWriter.WriteEndObject();
        }

        public char QuoteChar
        {
            get
            {
                return fJsonWriter.QuoteChar;
            }
            set
            {
                fJsonWriter.QuoteChar = value;
            }
        }

        public override void Close()
        {
            fJsonWriter.Close();
        }

        public override void Flush()
        {
            fJsonWriter.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return string.Empty;
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
        }

        public override void WriteCData(string text)
        {
        }

        public override void WriteCharEntity(char ch)
        {
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            WriteString(new string(buffer, index, count));
        }

        public override void WriteComment(string text)
        {
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
        }

        public override void WriteEndAttribute()
        {
        }

        public override void WriteEndDocument()
        {
        }

        public override void WriteEndElement()
        {
            WriteFullEndElement();
        }

        public override void WriteEntityRef(string name)
        {
        }

        public override void WriteFullEndElement()
        {
            switch (fLevel)
            {
                case 1:
                    if (!string.IsNullOrEmpty(fOldTableName))
                        fJsonWriter.WriteEndArray();
                    if (fActionResult != null)
                        WriteActionResult();

                    fJsonWriter.WriteEndObject();
                    fState = WriteState.Closed;
                    break;

                case 2:
                    fJsonWriter.WriteEndObject();
                    break;

                case 3:
                    if (fState == WriteState.Element)
                        fJsonWriter.WriteValue(string.Empty);
                    break;

                default:
                    break;
            }
            --fLevel;
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
        }

        public override void WriteRaw(string data)
        {
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
        }

        public override void WriteStartDocument(bool standalone)
        {
        }

        public override void WriteStartDocument()
        {
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            fState = WriteState.Element;
            ++fLevel;
            switch (fLevel)
            {
                case 1:
                    fJsonWriter.WriteStartObject();
                    break;

                case 2:
                    if (fOldTableName != localName)
                    {
                        if (!string.IsNullOrEmpty(fOldTableName))
                            fJsonWriter.WriteEndArray();
                        fOldTableName = localName;
                        fJsonWriter.WritePropertyName(localName);
                        fJsonWriter.WriteStartArray();
                    }
                    fJsonWriter.WriteStartObject();
                    break;

                case 3:
                    fJsonWriter.WritePropertyName(localName);
                    break;

                default:
                    break;
            }
        }

        public override WriteState WriteState
        {
            get
            {
                return fState;
            }
        }

        public override void WriteString(string text)
        {
            fState = WriteState.Content;
            fJsonWriter.WriteValue(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
        }

        public override void WriteWhitespace(string ws)
        {
        }
    }
}