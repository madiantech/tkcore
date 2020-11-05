using System;
using System.Xml;

namespace YJC.Toolkit.Xml
{
    public abstract class CustomElementXmlReader : XmlReader
    {
        private readonly XmlNameTable fNameTable;

        protected CustomElementXmlReader()
        {
            fNameTable = new NameTable();
            EmptyString = fNameTable.Add(string.Empty);
            State = XmlReaderState.Initial;
        }

        internal XmlReaderState State { get; set; }

        protected string EmptyString { get; private set; }

        public sealed override int AttributeCount
        {
            get
            {
                return 0;
            }
        }

        public override string BaseURI
        {
            get
            {
                return EmptyString;
            }
        }

        public sealed override int Depth
        {
            get
            {
                switch (State)
                {
                    case XmlReaderState.Row:
                    case XmlReaderState.EndRow:
                        return 1;
                    case XmlReaderState.Field:
                    case XmlReaderState.EndField:
                        return 2;
                    case XmlReaderState.FieldValue:
                        return 3;
                }
                return 0;
            }
        }

        public sealed override bool EOF
        {
            get
            {
                return State == XmlReaderState.Eof;
            }
        }

        public sealed override string GetAttribute(int i)
        {
            return null;
        }

        public sealed override string GetAttribute(string name, string namespaceURI)
        {
            return null;
        }

        public sealed override string GetAttribute(string name)
        {
            return null;
        }

        public sealed override bool HasValue
        {
            get
            {
                if (State == XmlReaderState.FieldValue)
                    return !string.IsNullOrEmpty(Value);
                return false;
            }
        }

        public sealed override string LookupNamespace(string prefix)
        {
            return null;
        }

        public sealed override bool MoveToAttribute(string name, string ns)
        {
            return false;
        }

        public sealed override bool MoveToAttribute(string name)
        {
            return false;
        }

        public sealed override bool MoveToElement()
        {
            if (State == XmlReaderState.Root || State == XmlReaderState.EndRoot || State == XmlReaderState.Row)
                return true;
            return false;
        }

        public sealed override bool MoveToFirstAttribute()
        {
            return false;
        }

        public sealed override bool MoveToNextAttribute()
        {
            return false;
        }

        public sealed override XmlNameTable NameTable
        {
            get
            {
                return fNameTable;
            }
        }

        public sealed override string NamespaceURI
        {
            get
            {
                return EmptyString;
            }
        }

        public sealed override XmlNodeType NodeType
        {
            get
            {
                switch (State)
                {
                    case XmlReaderState.Initial:
                    case XmlReaderState.Eof:
                        return XmlNodeType.None;
                    case XmlReaderState.Root:
                    case XmlReaderState.Row:
                    case XmlReaderState.Field:
                        return XmlNodeType.Element;
                    case XmlReaderState.FieldValue:
                        return XmlNodeType.Text;
                    case XmlReaderState.EndRoot:
                    case XmlReaderState.EndRow:
                    case XmlReaderState.EndField:
                        return XmlNodeType.EndElement;
                }
                return XmlNodeType.None;
            }
        }

        public sealed override string Prefix
        {
            get
            {
                return EmptyString;
            }
        }

        public sealed override bool ReadAttributeValue()
        {
            throw new NotSupportedException();
        }

        public sealed override ReadState ReadState
        {
            get
            {
                switch (State)
                {
                    case XmlReaderState.Initial:
                        return ReadState.Initial;
                    case XmlReaderState.Eof:
                        return ReadState.EndOfFile;
                    case XmlReaderState.Close:
                        return ReadState.Closed;
                    default:
                        return ReadState.Interactive;
                }
            }
        }

        public override void ResolveEntity()
        {
            throw new NotSupportedException();
        }
    }
}
