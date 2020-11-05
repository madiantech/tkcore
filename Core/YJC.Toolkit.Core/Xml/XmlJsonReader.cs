using System.IO;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Sys.Json;

namespace YJC.Toolkit.Xml
{
    /// <summary>
    /// 样例格式
    /// {TableName1: [{ FieldName1:"1", FieldName2:"2"}], TableName2: [{FieldName3:"3",
    /// FieldName4:"4"}, {FieldName3:"5", FieldName4:"6"}]}
    /// </summary>
    public sealed class XmlJsonReader : CustomElementXmlReader
    {
        private readonly Stream fSource;
        private readonly JsonTextReader fReader;
        private readonly string fRoot;
        private string fTableName;
        private string fFieldName;
        private string fFieldValue;
        private bool fReadTableName;
        private bool fIsEmpty;

        public XmlJsonReader(Stream source)
            : this(source, Encoding.UTF8)
        {
        }

        public XmlJsonReader(Stream source, Encoding encoding)
        {
            TkDebug.AssertArgumentNull(source, "source", null);
            TkDebug.AssertArgumentNull(encoding, "encoding", null);

            fSource = source;
            fReader = new JsonTextReader(new StreamReader(source, encoding));
            fRoot = NameTable.Add(ToolkitConst.ROOT_NODE_NAME);
        }

        private void AssertRead()
        {
            if (!fReader.Read())
                TkDebug.ThrowToolkitException(
                    "需要读取Json数据时，却无法读出，请确认数据是否完整", this);
        }

        private void AssertReadState(JsonToken token)
        {
            if (fReader.TokenType != token)
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "Json数据的状态有问题，当前需要的状态是{0}，实际状态却是{1}，在第{2}行，第{3}列",
                    token, fReader.TokenType, fReader.CurrentLineNumber, fReader.CurrentLinePosition), this);
        }

        private void AssertReadState(JsonToken token1, JsonToken token2)
        {
            if (fReader.TokenType != token1 && fReader.TokenType != token2)
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "Json数据的状态有问题，当前需要的状态是{0}或{1}，实际状态却是{2}，在第{3}行，第{4}列",
                    token1, token2, fReader.TokenType, fReader.CurrentLineNumber, fReader.CurrentLinePosition), this);
        }

        //private void AssertReadState(JsonToken token1, JsonToken token2, JsonToken token3)
        //{
        //    if (fReader.TokenType != token1 && fReader.TokenType != token2 && fReader.TokenType != token3)
        //        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
        //            "Json数据的状态有问题，当前需要的状态是{0}或{1}或{5}，实际状态却是{2}，在第{3}行，第{4}列",
        //            token1, token2, fReader.TokenType, fReader.CurrentLineNumber, fReader.CurrentLinePosition, token3), this);
        //}

        private void AssertReadState(params JsonToken[] tokens)
        {
            var result = tokens.Any(token => fReader.TokenType == token);
            if (!result)
            {
                string hint = string.Join("或", tokens);
                string message = $"Json数据的状态有问题，当前需要的状态是{hint}，实际状态却是{fReader.TokenType}，"
                    + $"在第{fReader.CurrentLineNumber}行，第{fReader.CurrentLinePosition}列";
                TkDebug.ThrowToolkitException(message, this);
            }
        }

        private void ReadNextRow()
        {
            AssertRead();
            AssertReadState(JsonToken.StartObject, JsonToken.EndArray);
            if (fReader.TokenType == JsonToken.StartObject)
                State = XmlReaderState.Row;
            else if (fReader.TokenType == JsonToken.EndArray)
                ReadTableName();
        }

        private void ReadFieldName()
        {
            State = XmlReaderState.Field;
            fFieldName = fReader.Value as string;
        }

        private void ReadTableName()
        {
            AssertRead();
            AssertReadState(JsonToken.PropertyName, JsonToken.EndObject);
            if (fReader.TokenType == JsonToken.PropertyName)
            {
                State = XmlReaderState.Row;
                fTableName = fReader.Value as string;
                fReadTableName = true;
                AssertRead();
                AssertReadState(JsonToken.StartArray);
                AssertRead();
                //var next = fReader.PeekNext();
                fIsEmpty = fReader.TokenType == JsonToken.EndArray;
            }
            else if (fReader.TokenType == JsonToken.EndObject)
                State = XmlReaderState.EndRoot;
        }

        public override void Close()
        {
            fSource.Close();
            fReader.Close();
        }

        public override bool IsEmptyElement
        {
            get
            {
                return fReadTableName ? fIsEmpty : false;
            }
        }

        public override string LocalName
        {
            get
            {
                switch (State)
                {
                    case XmlReaderState.Field:
                        return NameTable.Add(fFieldName);

                    case XmlReaderState.EndField:
                        return NameTable.Get(fFieldName);

                    case XmlReaderState.Root:
                    case XmlReaderState.EndRoot:
                        return fRoot;

                    case XmlReaderState.Row:
                        return NameTable.Add(fTableName);

                    case XmlReaderState.EndRow:
                        return NameTable.Get(fTableName);
                }
                return EmptyString;
            }
        }

        public override bool Read()
        {
            switch (State)
            {
                case XmlReaderState.Initial:
                    AssertRead();
                    AssertReadState(JsonToken.StartObject);
                    State = XmlReaderState.Root;
                    fReadTableName = false;
                    return true;

                case XmlReaderState.Root:
                    ReadTableName();
                    return true;

                case XmlReaderState.Row:
                    AssertReadState(JsonToken.StartObject, JsonToken.EndArray);
                    if (fReader.TokenType == JsonToken.StartObject)
                    {
                        AssertRead();
                        fReadTableName = false;
                        AssertReadState(JsonToken.PropertyName, JsonToken.EndObject);
                        if (fReader.TokenType == JsonToken.PropertyName)
                        {
                            ReadFieldName();
                        }
                        else if (fReader.TokenType == JsonToken.EndObject)
                        {
                            //fIsEmptyRow = true;
                            //ReadNextRow();
                            State = XmlReaderState.EndRow;
                        }
                    }
                    else if (fReader.TokenType == JsonToken.EndArray)
                    {
                        ReadTableName();
                    }
                    return true;

                case XmlReaderState.Field:
                    AssertRead();
                    AssertReadState(JsonToken.String, JsonToken.Integer, JsonToken.Null, JsonToken.Float, JsonToken.Boolean);
                    switch (fReader.TokenType)
                    {
                        case JsonToken.String:
                            fFieldValue = fReader.Value as string;
                            break;

                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.Boolean:
                            fFieldValue = fReader.Value.ToString();
                            break;

                        case JsonToken.Null:
                            fFieldValue = null;
                            break;
                    }
                    State = XmlReaderState.FieldValue;
                    return true;

                case XmlReaderState.FieldValue:
                    State = XmlReaderState.EndField;
                    return true;

                case XmlReaderState.EndField:
                    AssertRead();
                    AssertReadState(JsonToken.PropertyName, JsonToken.EndObject);
                    if (fReader.TokenType == JsonToken.PropertyName)
                        ReadFieldName();
                    else if (fReader.TokenType == JsonToken.EndObject)
                        State = XmlReaderState.EndRow;
                    return true;

                case XmlReaderState.EndRow:
                    ReadNextRow();
                    return true;

                case XmlReaderState.EndRoot:
                    State = XmlReaderState.Eof;
                    return true;

                case XmlReaderState.Eof:
                    return false;
            }
            return false;
        }

        public override string Value
        {
            get
            {
                if (State == XmlReaderState.FieldValue)
                    return fFieldValue;
                return null;
            }
        }
    }
}